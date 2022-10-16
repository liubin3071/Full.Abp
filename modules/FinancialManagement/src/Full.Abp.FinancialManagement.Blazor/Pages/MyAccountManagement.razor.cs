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

[Authorize]
public partial class MyAccountManagement
{
    [Inject] public IUserAccountAppService AppService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    protected List<TableColumn> Columns => TableColumns.Get<TenantAccountManagement>();
    protected PageToolbar Toolbar { get; } = new();
    protected List<BreadcrumbItem> BreadcrumbItems = new(2);
    protected IReadOnlyList<AccountDto> Entities = Array.Empty<AccountDto>();
    protected string CurrentSorting;
    protected TableColumnDictionary TableColumns { get; set; }
    protected AccountGetListInput GetListInput = new AccountGetListInput();
    protected EntityActionDictionary EntityActions { get; set; }


    public MyAccountManagement()
    {
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

}