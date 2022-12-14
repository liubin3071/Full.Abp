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
public partial class TenantAccountManagement
{
    public override string ProviderName => "T";
    protected override List<TableColumn> Columns => TableColumns.Get<TenantAccountManagement>();

    protected override ValueTask SetTableColumnsAsync()
    {
        Columns
            .AddRange(new TableColumn[] {
                new TableColumn { Title = L["Tenant"], Sortable = false, Component = typeof(TenantNameComponent) },
                new TableColumn { Title = L["Balance"], Sortable = true, Data = nameof(AccountDto.Balance), },
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

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<TenantAccountManagement>()
            .AddRange(new EntityAction[] {
                new EntityAction {
                    Text = L["Edit"],
                    Color = Color.Primary,
                    Visible = (data) => HasUpdatePermission,
                    Clicked = async (data) =>
                    {
                        await OpenEditModalAsync(data.As<AccountDto>());
                    }
                },
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
                    Clicked = async (data) =>
                    {
                        NavigationManager.NavigateTo(
                            $"/FinancialManagement/Accounts/{ProviderName}/{DefinitionName}/Entries/{data.As<AccountDto>().ProviderKey}");
                    },
                },

                // new EntityAction {
                //     Text = L["Edit"],
                //     Color = Color.Info,
                //     Visible = (data) => HasUpdatePermission,
                //     Clicked = async (data) => { await OpenEditModalAsync(data.As<CategoryDto>()); }
                // },
                // new EntityAction {
                //     Text = L["Delete"],
                //     Color = Color.Danger,
                //     Visible = (data) => HasDeletePermission, // && !data.As<PaymentQrcodeGetListOutput>().IsStatic,
                //     Clicked = async (data) => await DeleteEntityAsync(data.As<CategoryDto>()),
                //     ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<CategoryDto>())
                // }
            });

        return ValueTask.CompletedTask;
    }
}