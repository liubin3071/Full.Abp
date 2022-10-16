using Full.Abp.Finance.Accounts;
using Full.Abp.FinancialManagement.Localization;
using Full.Abp.FinancialManagement.Permissions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.UI.Navigation;

namespace Full.Abp.FinancialManagement.Blazor.Menus;

public class FinancialManagementMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();

        var l = context.GetLocalizer<FinancialManagementResource>();
        var financialManagementMenu =
            new ApplicationMenuItem("FinancialManagement", l["FinancialManagement"], icon: "fa fa-globe");
        administration.AddItem(financialManagementMenu);

        // 系统账户管理
        financialManagementMenu.AddItem(new ApplicationMenuItem(
            FinancialManagementMenus.SystemAccount,
            l["Menu:FinancialManagement:Accounts:SystemAccount"],
            $"/FinancialManagement/Accounts",
            "fa fa-dollar-sign fa-fw",
            requiredPermissionName: FinancialManagementPermissions.SystemAccounts.Default
        ));

        var tenantAccountMenu = new ApplicationMenuItem(FinancialManagementMenus.TenantAccount,
                l["Menu:FinancialManagement:Accounts:Tenants"],
                icon: "fa fa-wallet fa-fw");
        financialManagementMenu.AddItem(tenantAccountMenu);

        var userAccountMenu = new ApplicationMenuItem(FinancialManagementMenus.UserAccount, 
                l["Menu:FinancialManagement:Accounts:Users"],
                icon: "fa fa-users fa-fw");
        financialManagementMenu.AddItem(userAccountMenu);

        var accountDefinitionManager = context.ServiceProvider.GetRequiredService<IAccountDefinitionManager>();
        var accountDefinitions = accountDefinitionManager.GetAll();
        var currentTenant = context.ServiceProvider.GetRequiredService<ICurrentTenant>();
        var multiTenancySide = currentTenant.GetMultiTenancySide();

        foreach (var definition in accountDefinitions)
        {
            // 租户账户管理  如位开启多租户应移除`tenantAccountMenu`菜单
            if (!currentTenant.Id.HasValue && definition.IsAllowedProvider(TenantAccountProvider.ProviderName))
            {
                tenantAccountMenu.AddItem(new ApplicationMenuItem(definition.Name,
                    definition.DisplayName.Localize(context.StringLocalizerFactory),
                    url: $"/FinancialManagement/Accounts/{TenantAccountProvider.ProviderName}/{definition.Name}",
                    requiredPermissionName: FinancialManagementPermissions
                        .GetAccountManagementPermissions(TenantAccountProvider.ProviderName, definition.Name).Default
                ));
            }

            // 用户账户管理
            if (definition.IsAllowedProvider(UserAccountProvider.ProviderName))
            {
                userAccountMenu.AddItem(
                    new ApplicationMenuItem(
                        definition.Name,
                        definition.DisplayName.Localize(context.StringLocalizerFactory),
                        url: $"/FinancialManagement/Accounts/{UserAccountProvider.ProviderName}/{definition.Name}",
                        requiredPermissionName: FinancialManagementPermissions
                            .GetAccountManagementPermissions(UserAccountProvider.ProviderName, definition.Name)
                            .Default));
            }
        }

        return Task.CompletedTask;
    }
}