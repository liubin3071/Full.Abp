using Full.Abp.Finance.Accounts;
using Full.Abp.FinancialManagement.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Full.Abp.FinancialManagement.Permissions;

public class FinancialManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(FinancialManagementPermissions.GroupName, L("Permission:FinancialManagement"));
        var accountDefinitionManager = context.ServiceProvider.GetRequiredService<IAccountDefinitionManager>();
        var currentTenant = context.ServiceProvider.GetRequiredService<ICurrentTenant>();

        var systemAccountPermission = myGroup.AddPermission(FinancialManagementPermissions.SystemAccounts.Default, L("Permission:FinancialManagement:SystemAccount"));
        systemAccountPermission.AddChild(FinancialManagementPermissions.SystemAccounts.Increase,L("Permission:Increase"));
        systemAccountPermission.AddChild(FinancialManagementPermissions.SystemAccounts.Decrease,L("Permission:Decrease"));
        
        foreach (var definition in accountDefinitionManager.GetAll())
        {
            if (!currentTenant.Id.HasValue && definition.IsAllowedProvider(TenantAccountProvider.ProviderName))
            {
                var permissions =
                    FinancialManagementPermissions.GetAccountManagementPermissions(TenantAccountProvider.ProviderName,
                        definition.Name);

                var permission = myGroup.AddPermission(permissions.Default,
                    L($"Permission:FinancialManagement:Accounts:{TenantAccountProvider.ProviderName}:{definition.Name}"),
                    MultiTenancySides.Both);
                permission.AddChild(permissions.Create, L("Permission:Create"));
                permission.AddChild(permissions.Update, L("Permission:Update"));
                permission.AddChild(permissions.Increase, L("Permission:Increase"));
                permission.AddChild(permissions.Decrease, L("Permission:Decrease"));
            }

            if (definition.IsAllowedProvider(UserAccountProvider.ProviderName))
            {
                var permissions =
                    FinancialManagementPermissions.GetAccountManagementPermissions(UserAccountProvider.ProviderName,
                        definition.Name);

                var permission = myGroup.AddPermission(permissions.Default,
                    L($"Permission:FinancialManagement:Accounts:{UserAccountProvider.ProviderName}:{definition.Name}"),
                    MultiTenancySides.Both);
                permission.AddChild(permissions.Create, L("Permission:Create"));
                permission.AddChild(permissions.Update, L("Permission:Update"));
                permission.AddChild(permissions.Increase, L("Permission:Increase"));
                permission.AddChild(permissions.Decrease, L("Permission:Decrease"));
            }
        }
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FinancialManagementResource>(name);
    }
}