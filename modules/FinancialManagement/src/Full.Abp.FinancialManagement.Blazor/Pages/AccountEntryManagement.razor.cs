using Blazorise;
using Blazorise.DataGrid;
using Full.Abp.FinancialManagement.AccountEntries;
using Full.Abp.FinancialManagement.Accounts;
using Full.Abp.FinancialManagement.Localization;
using Full.Abp.FinancialManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Full.Abp.FinancialManagement.Blazor.Pages;

[Authorize]
public partial class AccountEntryManagement
{
    [Parameter]
    public string ProviderName { get; set; }

    [Parameter]
    public string ProviderKey { get; set; }

    [Parameter]
    public string DefinitionName { get; set; }
    
    [Inject] public IAccountEntryAppService AppService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    protected PageToolbar Toolbar { get; } = new();
    protected List<BreadcrumbItem> BreadcrumbItems = new(2);
    protected IReadOnlyList<AccountEntryGetListOutput> Entities = Array.Empty<AccountEntryGetListOutput>();
    protected int? TotalCount;
    protected int CurrentPage = 1;
    protected string CurrentSorting;

    protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    protected TableColumnDictionary TableColumns { get; set; }
    protected virtual List<TableColumn> Columns => TableColumns.Get<AccountEntryManagement>();
    protected AccountEntryGetListInput GetListInput = new AccountEntryGetListInput();
    protected EntityActionDictionary EntityActions { get; set; }

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

    public AccountEntryManagement()
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
        await SetEntityActionsAsync();
        await SetTableColumnsAsync();
        await InvokeAsync(StateHasChanged);
    }

    // protected override Task OnParametersSetAsync()
    // {
    //     GetListInput = new AccountEntryGetListInput();
    //     return SearchEntitiesAsync();
    // }
    protected virtual ValueTask SetTableColumnsAsync()
    {
        Columns
            .AddRange(new TableColumn[] {
                new TableColumn { Title = L["Amount"], Sortable = true, Data = nameof(AccountEntryGetListOutput.Amount), },
                new TableColumn { Title = L["PostBalance"], Sortable = true, Data = nameof(AccountEntryGetListOutput.PostBalance), },
                new TableColumn { Title = L["TransactionType"], Sortable = true, Data = nameof(AccountEntryGetListOutput.TransactionType), },
                new TableColumn { Title = L["TransactionId"], Sortable = true, Data = nameof(AccountEntryGetListOutput.TransactionId), },
                new TableColumn { Title = L["Comments"], Sortable = true, Data = nameof(AccountEntryGetListOutput.Comments), },
                new TableColumn { Title = L["CreationTime"], Sortable = true, Data = nameof(AccountEntryGetListOutput.CreationTime), },
                // new TableColumn { Title = L["Actions"], Actions = EntityActions.Get<AccountEntryManagement>(), },
            });

        return ValueTask.CompletedTask;
    }
    
    protected virtual ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<AccountEntryManagement>()
            .AddRange(new EntityAction[] {
                // new EntityAction {
                //     Text = L["Edit"], Color = Color.Primary, Visible = (data) => true, Clicked = async (data) =>
                //     {
                //         await OpenEditModalAsync(data.As<AccountDto>());
                //     }
                // },
            });

        return ValueTask.CompletedTask;
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
 
    protected virtual ValueTask SetToolbarItemsAsync()
    {
        // todo 导出
        // Toolbar.AddButton(L["NewAccount"],
        //     OpenCreateModalAsync,
        //     IconName.Add,
        //     requiredPolicyName: CreatePolicyName);
        return ValueTask.CompletedTask;
    }
    
    
    protected virtual async Task SearchEntitiesAsync()
    {
        CurrentPage = 1;
        await GetEntitiesAsync();
        await InvokeAsync(StateHasChanged);
    }
    
    protected virtual async Task GetEntitiesAsync()
    {
        try
        {
            await UpdateGetListInputAsync();
            var result = await AppService.GetListAsync(GetListInput);
            Entities = result.Items;
            TotalCount = (int?)result.TotalCount;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
    protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<AccountEntryGetListOutput> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page;

        await GetEntitiesAsync();

        await InvokeAsync(StateHasChanged);
    }
    protected virtual Task UpdateGetListInputAsync()
    {
        GetListInput.Name = DefinitionName;
        GetListInput.ProviderName = ProviderName;
        GetListInput.ProviderKey = ProviderKey ?? string.Empty;
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
}