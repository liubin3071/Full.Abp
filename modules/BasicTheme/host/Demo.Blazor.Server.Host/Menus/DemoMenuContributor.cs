using System.Threading.Tasks;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace Demo.Blazor.Server.Host.Menus;

public class DemoMenuContributor : IMenuContributor
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

        if (true)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);
        context.Menu.AddItem(
            new ApplicationMenuItem(
                "Administration111",
                "Menu:Administration",
                icon: "fa fa-wrench"
            )
        );
        var test = new ApplicationMenuItem("Demo", "Demo", icon: "Home");
        context.Menu.AddItem(test);
        
        test.AddItem(new ApplicationMenuItem("UiNotificationAlert", "UiNotificationAlert","/UiNotificationAlert"));
        return Task.CompletedTask;
    }
}