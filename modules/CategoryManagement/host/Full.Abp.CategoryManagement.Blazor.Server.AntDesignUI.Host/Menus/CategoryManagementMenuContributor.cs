using System.Threading.Tasks;
using Full.Abp.CategoryManagement.MultiTenancy;
using Full.Abp.IdentityManagement.Blazor.AntDesignUI;
using Full.Abp.SettingManagement.Blazor.AntDesignUI;
using Full.Abp.TenantManagement.Blazor.AntDesignUI;
using Volo.Abp.UI.Navigation;

namespace Full.Abp.CategoryDemo.Blazor.Server.Menus;

public class CategoryManagementMenuContributor : IMenuContributor
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

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

        return Task.CompletedTask;
    }
}
