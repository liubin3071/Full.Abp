using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Full.Abp.Categories;
using Full.Abp.Categories.Definitions;
using Full.Abp.CategoryManagement.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.Authorization;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Full.Abp.CategoryManagement.Blazor.Pages;

public partial class CategoryManagement
{
    [Parameter] public string CategoryDefinitionName { get; set; }
    
    [Inject] public ICategoryDefinitionManager CategoryDefinitionManager { get; set; }
    
    protected int? TotalCount;
    public int PageCount => (int)Math.Ceiling((TotalCount ?? 1d) / PageSize);
    protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

    protected PageToolbar Toolbar { get; } = new();

    // protected List<TableColumn> Columns => TableColumns.Get<CategoryManagement>();
    protected Type ObjectMapperContext { get; set; }
    protected string CreatePolicyName { get; set; }
    protected string UpdatePolicyName { get; set; }
    protected string DeletePolicyName { get; set; }
    public bool HasCreatePermission { get; set; }
    public bool HasUpdatePermission { get; set; }
    public bool HasDeletePermission { get; set; }
    [Inject] public ICategoryAppService CategoryAppService { get; set; }
    protected EntityActionDictionary EntityActions { get; set; }
    protected TableColumnDictionary TableColumns { get; set; }
    protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>(2);

    protected IReadOnlyList<CategoryDto> Entities = Array.Empty<CategoryDto>();
    protected CategoryCreateOrUpdateInput NewEntity;
    protected Guid EditingEntityId;
    protected CategoryCreateOrUpdateInput EditingEntity;
    protected Modal? CreateModal;
    protected Modal? EditModal;
    protected Validations? CreateValidationsRef;
    protected Validations? EditValidationsRef;

    public CategoryManagement()
    {
        NewEntity = new CategoryCreateOrUpdateInput();
        EditingEntity = new CategoryCreateOrUpdateInput();
        TableColumns = new TableColumnDictionary();
        EntityActions = new EntityActionDictionary();
        
        ObjectMapperContext = typeof(CategoryManagementBlazorModule);
        LocalizationResource = typeof(CategoryManagementResource);
    }

    protected async override Task OnInitializedAsync()
    {
        CreatePolicyName = $"CategoryManagement.{CategoryDefinitionName}.Create";
        UpdatePolicyName = $"CategoryManagement.{CategoryDefinitionName}.Update";
        DeletePolicyName = $"CategoryManagement.{CategoryDefinitionName}.Delete";
        await SetPermissionsAsync();
        await SetEntityActionsAsync();
        await SetTableColumnsAsync();

        await GetEntitiesAsync();
        await InvokeAsync(StateHasChanged);
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

    protected async ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["New"],
            () => OpenCreateModalAsync(null),
            IconName.Add,
            requiredPolicyName: CreatePolicyName);
        await InvokeAsync(StateHasChanged);
        await ValueTask.CompletedTask;
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

        if (DeletePolicyName != null)
        {
            HasDeletePermission = await AuthorizationService.IsGrantedAsync(DeletePolicyName);
        }
    }

    protected ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<CategoryManagement>()
            .AddRange(new EntityAction[] {
                new EntityAction {
                    Text = L["New"],
                    Color = Color.Primary,
                    Visible = (data) => HasUpdatePermission,
                    Clicked = async (data) => { await OpenCreateModalAsync(data.As<CategoryDto>().Id); }
                },
                new EntityAction {
                    Text = L["Edit"],
                    Color = Color.Info,
                    Visible = (data) => HasUpdatePermission,
                    Clicked = async (data) => { await OpenEditModalAsync(data.As<CategoryDto>()); }
                },
                new EntityAction {
                    Text = L["Delete"],
                    Color = Color.Danger,
                    Visible = (data) => HasDeletePermission, // && !data.As<PaymentQrcodeGetListOutput>().IsStatic,
                    Clicked = async (data) => await DeleteEntityAsync(data.As<CategoryDto>()),
                    ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<CategoryDto>())
                }
            });

        return ValueTask.CompletedTask;
    }

    protected List<TableColumn> Columns => TableColumns.Get<CategoryManagement>();

    protected ValueTask SetTableColumnsAsync()
    {
        Columns
            .AddRange(new TableColumn[] {
                new TableColumn {
                    Title = L["Name"], Sortable = true, Data = nameof(CategoryDto.Name),
                    // Component = typeof(RoleNameComponent)
                },
                new TableColumn { Title = L["Actions"], Actions = EntityActions.Get<CategoryManagement>(), },
            });

        // Columns.AddRange(GetExtensionTableColumns(IdentityModuleExtensionConsts.ModuleName,
        //     IdentityModuleExtensionConsts.EntityNames.Role));

        return ValueTask.CompletedTask;
    }

    protected virtual async Task GetEntitiesAsync()
    {
        try
        {
            // await UpdateGetListInputAsync();
            var result = await CategoryAppService.GetChildrenAsync(CategoryDefinitionName, null);
            Entities = (result.Items);
            // TotalCount = (int?)result.TotalCount;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }


    protected virtual async Task OpenCreateModalAsync(Guid? parentId)
    {
        try
        {
            if (CreateValidationsRef != null)
            {
                await CreateValidationsRef.ClearAll();
            }

            await CheckCreatePolicyAsync();

            NewEntity = new CategoryCreateOrUpdateInput() { ParentId = parentId };

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

    protected virtual Task CloseCreateModalAsync()
    {
        NewEntity = new CategoryCreateOrUpdateInput();
        return InvokeAsync(CreateModal!.Hide);
    }

    protected virtual Task ClosingCreateModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }

    protected virtual CategoryCreateOrUpdateInput MapToEditingEntity(CategoryDto entityDto)
    {
        return ObjectMapper.Map<CategoryDto, CategoryCreateOrUpdateInput>(entityDto);
    }

    protected virtual async Task OpenEditModalAsync(CategoryDto entity)
    {
        try
        {
            if (EditValidationsRef != null)
            {
                await EditValidationsRef.ClearAll();
            }

            await CheckUpdatePolicyAsync();

            var entityDto = await CategoryAppService.GetAsync(CategoryDefinitionName, entity.Id);

            EditingEntityId = entity.Id;
            EditingEntity = MapToEditingEntity(entityDto);
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


    protected virtual Task CloseEditModalAsync()
    {
        InvokeAsync(EditModal.Hide);
        return Task.CompletedTask;
    }

    protected virtual Task ClosingEditModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
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
                var createInput = (NewEntity);
                await CategoryAppService.CreateAsync(CategoryDefinitionName, createInput);

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
        return Task.CompletedTask;
    }

    protected virtual async Task OnCreatedEntityAsync()
    {
        NewEntity = new CategoryCreateOrUpdateInput();
        await GetEntitiesAsync();

        await InvokeAsync(CreateModal!.Hide);
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
                var updateInput = (EditingEntity);
                await CategoryAppService.UpdateAsync(CategoryDefinitionName, EditingEntityId, updateInput);

                await OnUpdatedEntityAsync();
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

    protected virtual async Task DeleteEntityAsync(CategoryDto entity)
    {
        try
        {
            await CheckDeletePolicyAsync();
            await OnDeletingEntityAsync();
            await CategoryAppService.DeleteAsync(CategoryDefinitionName, entity.Id);
            await OnDeletedEntityAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual Task OnDeletingEntityAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual async Task OnDeletedEntityAsync()
    {
        await GetEntitiesAsync();
        await InvokeAsync(StateHasChanged);
        await Notify.Success(L["SuccessfullyDeleted"]);
    }

    protected virtual string GetDeleteConfirmationMessage(CategoryDto entity)
    {
        return string.Format(L["CategoryDeletionConfirmationMessage"], entity.Name);
    }

    protected virtual async Task CheckCreatePolicyAsync()
    {
        await CheckPolicyAsync(CreatePolicyName);
    }

    protected virtual async Task CheckUpdatePolicyAsync()
    {
        await CheckPolicyAsync(UpdatePolicyName);
    }

    protected virtual async Task CheckDeletePolicyAsync()
    {
        await CheckPolicyAsync(DeletePolicyName);
    }

    /// <summary>
    /// Calls IAuthorizationService.CheckAsync for the given <paramref name="policyName"/>.
    /// Throws <see cref="AbpAuthorizationException"/> if given policy was not granted for the current user.
    ///
    /// Does nothing if <paramref name="policyName"/> is null or empty.
    /// </summary>
    /// <param name="policyName">A policy name to check</param>
    protected virtual async Task CheckPolicyAsync(string? policyName)
    {
        if (string.IsNullOrEmpty(policyName))
        {
            return;
        }

        await AuthorizationService.CheckAsync(policyName);
    }

    private async Task<IEnumerable<CategoryDto>> GetChildNodesAsync(CategoryDto category)
    {
        var result = await CategoryAppService.GetChildrenAsync(CategoryDefinitionName, category.Id);
        return result.Items;
    }
}