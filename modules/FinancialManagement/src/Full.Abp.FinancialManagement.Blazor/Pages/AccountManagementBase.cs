using Blazorise;
using Blazorise.DataGrid;
using Full.Abp.FinancialManagement.Accounts;
using Full.Abp.FinancialManagement.Localization;
using Full.Abp.FinancialManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.Authorization;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Full.Abp.FinancialManagement.Blazor.Pages;

public abstract class AccountManagementBase : AbpComponentBase
{
    [Parameter] public string DefinitionName { get; set; }
    public abstract string ProviderName { get; }

    [Inject] public IAccountAppService AppService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    protected PageToolbar Toolbar { get; } = new();
    protected List<BreadcrumbItem> BreadcrumbItems = new(2);
    protected IReadOnlyList<AccountDto> Entities = Array.Empty<AccountDto>();
    protected int? TotalCount;
    protected int CurrentPage = 1;
    protected string CurrentSorting;

    protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    protected TableColumnDictionary TableColumns { get; set; }
    protected abstract List<TableColumn> Columns { get; }
    protected AccountGetListInput GetListInput = new AccountGetListInput();
    protected EntityActionDictionary EntityActions { get; set; }
    protected string CreatePolicyName { get; set; }

    protected string UpdatePolicyName { get; set; }
    protected string IncreasePolicyName { get; set; }
    protected string DecreasePolicyName { get; set; }


    public bool HasCreatePermission { get; set; }

    public bool HasUpdatePermission { get; set; }
    public bool HasIncreasePermission { get; set; }
    public bool HasDecreasePermission { get; set; }

    protected Modal? CreateModal;
    protected Modal? EditModal;
    protected Modal? IncreaseModal;
    protected Modal? DecreaseModal;

    protected Validations? CreateValidationsRef;
    protected Validations? EditValidationsRef;
    protected Validations? IncreaseValidationsRef;
    protected Validations? DecreaseValidationsRef;

    protected AccountCreateOrUpdateInput NewEntity;
    protected AccountCreateOrUpdateInput EditingEntity;

    protected AccountIncreaseInput IncreaseEntity;
    protected AccountDecreaseInput DecreaseEntity;

    public AccountManagementBase()
    {
        NewEntity = new AccountCreateOrUpdateInput();
        EditingEntity = new AccountCreateOrUpdateInput();
        IncreaseEntity = new AccountIncreaseInput();
        DecreaseEntity = new AccountDecreaseInput();

        TableColumns = new TableColumnDictionary();
        EntityActions = new EntityActionDictionary();

        ObjectMapperContext = typeof(FinancialManagementBlazorModule);
        LocalizationResource = typeof(FinancialManagementResource);
    }

    protected async override Task OnInitializedAsync()
    {
        var permission = FinancialManagementPermissions.GetAccountManagementPermissions("U", DefinitionName);
        CreatePolicyName = permission.Create;
        UpdatePolicyName = permission.Update;
        IncreasePolicyName = permission.Increase;
        DecreasePolicyName = permission.Decrease;

        await SetPermissionsAsync();
        await SetEntityActionsAsync();
        await SetTableColumnsAsync();
        await InvokeAsync(StateHasChanged);
    }

    protected override Task OnParametersSetAsync()
    {
        GetListInput = new AccountGetListInput();
        return SearchEntitiesAsync();
    }

    protected virtual async Task SetPermissionsAsync()
    {
        if (CreatePolicyName != null)
        {
            HasCreatePermission = await AuthorizationService.IsGrantedAsync(CreatePolicyName);
        }

        if (UpdatePolicyName != null)
        {
            HasUpdatePermission = await AuthorizationService.IsGrantedAsync(UpdatePolicyName);
        }

        if (IncreasePolicyName != null)
        {
            HasIncreasePermission = await AuthorizationService.IsGrantedAsync(IncreasePolicyName);
        }

        if (DecreasePolicyName != null)
        {
            HasDecreasePermission = await AuthorizationService.IsGrantedAsync(DecreasePolicyName);
        }
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
        }
    }

    protected virtual ValueTask SetBreadcrumbItemsAsync()
    {
        return ValueTask.CompletedTask;
    }

    protected abstract ValueTask SetEntityActionsAsync();

    protected abstract ValueTask SetTableColumnsAsync();

    protected virtual ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewAccount"],
            OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);
        return ValueTask.CompletedTask;
    }

    protected virtual async Task OpenCreateModalAsync()
    {
        try
        {
            if (CreateValidationsRef != null)
            {
                await CreateValidationsRef.ClearAll();
            }

            await CheckCreatePolicyAsync();

            NewEntity = new AccountCreateOrUpdateInput() { Name = this.DefinitionName, ProviderName = ProviderName };

            // Mapper will not notify Blazor that binded values are changed
            // so we need to notify it manually by calling StateHasChanged
            await InvokeAsync(async () =>
            {
                StateHasChanged();
                if (CreateModal != null)
                {
                    await CreateModal.Show();
                }
            });
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task CreateEntityAsync()
    {
        try
        {
            var validate = true;
            if (CreateValidationsRef != null)
            {
                validate = await CreateValidationsRef.ValidateAll();
            }

            if (validate)
            {
                await OnCreatingEntityAsync();

                await CheckCreatePolicyAsync();
                await AppService.CreateOrUpdateAsync(NewEntity);

                await OnCreatedEntityAsync();
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual Task OnCreatingEntityAsync()
    {
        NewEntity.Name = DefinitionName;
        NewEntity.ProviderName = ProviderName;
        return Task.CompletedTask;
    }

    protected virtual async Task OnCreatedEntityAsync()
    {
        NewEntity = new AccountCreateOrUpdateInput();
        await GetEntitiesAsync();

        await InvokeAsync(CreateModal.Hide);
    }

    protected virtual Task CloseCreateModalAsync()
    {
        NewEntity = new AccountCreateOrUpdateInput();
        return InvokeAsync(CreateModal.Hide);
    }

    protected virtual Task ClosingCreateModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }

    protected virtual async Task OpenEditModalAsync(AccountDto entity)
    {
        try
        {
            if (EditValidationsRef != null)
            {
                await EditValidationsRef.ClearAll();
            }

            await CheckUpdatePolicyAsync();

            var entityDto = await AppService.GetAsync(new AccountGetInput() {
                Name = DefinitionName, ProviderName = ProviderName, ProviderKey = entity.ProviderKey
            });

            EditingEntity = new AccountCreateOrUpdateInput() {
                Name = DefinitionName,
                ProviderName = ProviderName,
                ProviderKey = entityDto.ProviderKey,
                IsEnabled = entityDto.IsEnabled
            };

            await InvokeAsync(async () =>
            {
                StateHasChanged();
                if (EditModal != null)
                {
                    await EditModal.Show();
                }
            });
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }


    protected virtual async Task OpenIncreaseModalAsync(AccountDto entity)
    {
        try
        {
            if (IncreaseValidationsRef != null)
            {
                await IncreaseValidationsRef.ClearAll();
            }

            await CheckIncreasePolicyAsync();

            IncreaseEntity = new AccountIncreaseInput() {
                Name = DefinitionName, ProviderName = ProviderName, ProviderKey = entity.ProviderKey,
            };

            await InvokeAsync(async () =>
            {
                StateHasChanged();
                if (IncreaseModal != null)
                {
                    await IncreaseModal.Show();
                }
            });
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }


    protected virtual async Task OpenDecreaseModalAsync(AccountDto entity)
    {
        try
        {
            if (DecreaseValidationsRef != null)
            {
                await DecreaseValidationsRef.ClearAll();
            }

            await CheckDecreasePolicyAsync();

            DecreaseEntity = new AccountDecreaseInput() {
                Name = DefinitionName, ProviderName = ProviderName, ProviderKey = entity.ProviderKey,
            };

            await InvokeAsync(async () =>
            {
                StateHasChanged();
                if (DecreaseModal != null)
                {
                    await DecreaseModal.Show();
                }
            });
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }


    protected virtual Task CloseEditModalAsync()
    {
        InvokeAsync(EditModal.Hide);
        return Task.CompletedTask;
    }

    protected virtual Task CloseIncreaseModalAsync()
    {
        InvokeAsync(IncreaseModal.Hide);
        return Task.CompletedTask;
    }

    protected virtual Task CloseDecreaseModalAsync()
    {
        InvokeAsync(DecreaseModal.Hide);
        return Task.CompletedTask;
    }

    protected virtual Task ClosingEditModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }

    protected virtual Task ClosingIncreaseModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }

    protected virtual Task ClosingDecreaseModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }

    protected virtual async Task UpdateEntityAsync()
    {
        try
        {
            var validate = true;
            if (EditValidationsRef != null)
            {
                validate = await EditValidationsRef.ValidateAll();
            }

            if (validate)
            {
                await OnUpdatingEntityAsync();

                await CheckUpdatePolicyAsync();
                await AppService.CreateOrUpdateAsync(EditingEntity);

                await OnUpdatedEntityAsync();
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task IncreaseAsync()
    {
        try
        {
            var validate = true;
            if (IncreaseValidationsRef != null)
            {
                validate = await IncreaseValidationsRef.ValidateAll();
            }

            if (validate)
            {
                await CheckIncreasePolicyAsync();
                await AppService.IncreaseAsync(IncreaseEntity);

                await GetEntitiesAsync();
                await InvokeAsync(IncreaseModal.Hide);
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task DecreaseAsync()
    {
        try
        {
            var validate = true;
            if (DecreaseValidationsRef != null)
            {
                validate = await DecreaseValidationsRef.ValidateAll();
            }

            if (validate)
            {
                await CheckDecreasePolicyAsync();
                await AppService.DecreaseAsync(DecreaseEntity);

                await GetEntitiesAsync();
                await InvokeAsync(DecreaseModal.Hide);
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual Task OnUpdatingEntityAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual async Task OnUpdatedEntityAsync()
    {
        await GetEntitiesAsync();

        await InvokeAsync(EditModal.Hide);
    }

    protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<AccountDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page;

        await GetEntitiesAsync();

        await InvokeAsync(StateHasChanged);
    }


    protected virtual async Task GetEntitiesAsync()
    {
        try
        {
            await UpdateGetListInputAsync();
            var result = await AppService.GetEntriesAsync(GetListInput);
            Entities = result.Items;
            TotalCount = (int?)result.TotalCount;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual Task UpdateGetListInputAsync()
    {
        GetListInput.Name = DefinitionName;
        GetListInput.ProviderName = ProviderName;
        if (GetListInput is ISortedResultRequest sortedResultRequestInput)
        {
            sortedResultRequestInput.Sorting = CurrentSorting;
        }

        if (GetListInput is IPagedResultRequest pagedResultRequestInput)
        {
            pagedResultRequestInput.SkipCount = (CurrentPage - 1) * PageSize;
        }

        if (GetListInput is ILimitedResultRequest limitedResultRequestInput)
        {
            limitedResultRequestInput.MaxResultCount = PageSize;
        }

        return Task.CompletedTask;
    }

    protected virtual async Task CheckCreatePolicyAsync()
    {
        await CheckPolicyAsync(CreatePolicyName);
    }

    protected virtual async Task CheckUpdatePolicyAsync()
    {
        await CheckPolicyAsync(UpdatePolicyName);
    }

    protected virtual async Task CheckIncreasePolicyAsync()
    {
        await CheckPolicyAsync(IncreasePolicyName);
    }

    protected virtual async Task CheckDecreasePolicyAsync()
    {
        await CheckPolicyAsync(DecreasePolicyName);
    }

    /// <summary>
    /// Calls IAuthorizationService.CheckAsync for the given <paramref name="policyName"/>.
    /// Throws <see cref="AbpAuthorizationException"/> if given policy was not granted for the current user.
    ///
    /// Does nothing if <paramref name="policyName"/> is null or empty.
    /// </summary>
    /// <param name="policyName">A policy name to check</param>
    protected virtual async Task CheckPolicyAsync(string policyName)
    {
        if (string.IsNullOrEmpty(policyName))
        {
            return;
        }

        await AuthorizationService.CheckAsync(policyName);
    }

    protected virtual async Task SearchEntitiesAsync()
    {
        CurrentPage = 1;
        await GetEntitiesAsync();
        await InvokeAsync(StateHasChanged);
    }

    #region view entries modal

    protected Modal ViewEntriesModal;

    protected virtual async Task OnViewEntriesDataGridReadAsync(DataGridReadDataEventArgs<AccountDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page;

        await GetEntitiesAsync();

        await InvokeAsync(StateHasChanged);
    }
    protected virtual async Task OpenViewEntriesModalAsync()
    {
        try
        {
            if (CreateValidationsRef != null)
            {
                await CreateValidationsRef.ClearAll();
            }

            await CheckCreatePolicyAsync();

            NewEntity = new AccountCreateOrUpdateInput() { Name = this.DefinitionName, ProviderName = ProviderName };

            // Mapper will not notify Blazor that binded values are changed
            // so we need to notify it manually by calling StateHasChanged
            await InvokeAsync(async () =>
            {
                StateHasChanged();
                if (CreateModal != null)
                {
                    await CreateModal.Show();
                }
            });
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual Task CloseViewEntriesModalAsync()
    {
        InvokeAsync(ViewEntriesModal.Hide);
        return Task.CompletedTask;
    }

    protected virtual Task ClosingViewEntriesModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }

    #endregion
}