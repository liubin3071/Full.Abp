using Blazorise;
using Blazorise.DataGrid;
using Full.Abp.FinancialManagement.Accounts;
using Full.Abp.FinancialManagement.Localization;
using Full.Abp.FinancialManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.Authorization;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Full.Abp.FinancialManagement.Blazor.Pages;

[Authorize(FinancialManagementPermissions.SystemAccounts.Default)]
public partial class SystemAccountManagement
{
    [Inject] public ISystemAccountAppService AppService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    protected List<TableColumn> Columns => TableColumns.Get<TenantAccountManagement>();
    protected PageToolbar Toolbar { get; } = new();
    protected List<BreadcrumbItem> BreadcrumbItems = new(2);
    protected IReadOnlyList<AccountDto> Entities = Array.Empty<AccountDto>();
    protected string CurrentSorting;
    protected TableColumnDictionary TableColumns { get; set; }
    protected AccountGetListInput GetListInput = new AccountGetListInput();
    protected EntityActionDictionary EntityActions { get; set; }
    protected Modal? IncreaseModal;
    protected Modal? DecreaseModal;
    protected Validations? IncreaseValidationsRef;
    protected Validations? DecreaseValidationsRef;
    protected SystemAccountIncreaseInput IncreaseEntity;
    protected SystemAccountDecreaseInput DecreaseEntity;
    protected string IncreasePolicyName { get; set; }
    protected string DecreasePolicyName { get; set; }

    public bool HasIncreasePermission { get; set; }
    public bool HasDecreasePermission { get; set; }

    public SystemAccountManagement()
    {
        IncreaseEntity = new SystemAccountIncreaseInput();
        DecreaseEntity = new SystemAccountDecreaseInput();

        TableColumns = new TableColumnDictionary();
        EntityActions = new EntityActionDictionary();
        ObjectMapperContext = typeof(FinancialManagementBlazorModule);
        LocalizationResource = typeof(FinancialManagementResource);
    }

    protected async override Task OnInitializedAsync()
    {
        IncreasePolicyName = FinancialManagementPermissions.SystemAccounts.Increase;
        DecreasePolicyName = FinancialManagementPermissions.SystemAccounts.Decrease;

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

    protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<AccountDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");

        await GetEntitiesAsync();
        await InvokeAsync(StateHasChanged);
    }

    protected ValueTask SetTableColumnsAsync()
    {
        Columns
            .AddRange(new TableColumn[] {
                new TableColumn { Title = L["AccountName"], Sortable = false, Data = nameof(AccountDto.DisplayName) },
                new TableColumn { Title = L["Balance"], Sortable = false, Data = nameof(AccountDto.Balance), },
                new TableColumn {
                    Title = L["IsEnabled"],
                    Sortable = true,
                    Data = nameof(AccountDto.IsEnabled),
                    Component = typeof(AccountEnabledComponent)
                },
                new TableColumn { Title = L["Actions"], Actions = EntityActions.Get<TenantAccountManagement>(), },
            });

        return ValueTask.CompletedTask;
    }

    protected ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<TenantAccountManagement>()
            .AddRange(new EntityAction[] {
                new EntityAction {
                    Text = L["Increase"],
                    Color = Color.Primary,
                    Visible = (data) => HasIncreasePermission,
                    Clicked = async (data) =>
                    {
                        await OpenIncreaseModalAsync(data.As<AccountDto>());
                    }
                },
                new EntityAction {
                    Text = L["Decrease"],
                    Color = Color.Primary,
                    Visible = (data) => HasDecreasePermission,
                    Clicked = async (data) =>
                    {
                        await OpenDecreaseModalAsync(data.As<AccountDto>());
                    }
                },
                new EntityAction {
                    Text = L["ViewEntries"],
                    Color = Color.Primary,
                    Visible = (data) => true,
                    Clicked = (data) =>
                    {
                        var account = data.As<AccountDto>();
                        NavigationManager.NavigateTo(
                            $"/FinancialManagement/Accounts/{account.ProviderName}/{account.Name}/Entries/{account.ProviderKey}");
                        return Task.CompletedTask;
                    }
                },
            });

        return ValueTask.CompletedTask;
    }

    protected virtual async Task SearchEntitiesAsync()
    {
        await GetEntitiesAsync();
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task GetEntitiesAsync()
    {
        try
        {
            await UpdateGetListInputAsync();
            var result = await AppService.GetListAsync();
            Entities = result.Items;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual Task UpdateGetListInputAsync()
    {
        if (GetListInput is ISortedResultRequest sortedResultRequestInput)
        {
            sortedResultRequestInput.Sorting = CurrentSorting;
        }

        return Task.CompletedTask;
    }

    protected virtual async Task SetPermissionsAsync()
    {
        if (IncreasePolicyName != null)
        {
            HasIncreasePermission = await AuthorizationService.IsGrantedAsync(IncreasePolicyName);
        }

        if (DecreasePolicyName != null)
        {
            HasDecreasePermission = await AuthorizationService.IsGrantedAsync(DecreasePolicyName);
        }
    }


    protected virtual async Task CheckPolicyAsync(string policyName)
    {
        if (string.IsNullOrEmpty(policyName))
        {
            return;
        }

        await AuthorizationService.CheckAsync(policyName);
    }

    protected virtual async Task CheckIncreasePolicyAsync()
    {
        await CheckPolicyAsync(IncreasePolicyName);
    }

    protected virtual async Task CheckDecreasePolicyAsync()
    {
        await CheckPolicyAsync(DecreasePolicyName);
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

            IncreaseEntity = new SystemAccountIncreaseInput() { Name = entity.Name, };

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

            DecreaseEntity = new SystemAccountDecreaseInput() { Name = entity.Name, };

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
}