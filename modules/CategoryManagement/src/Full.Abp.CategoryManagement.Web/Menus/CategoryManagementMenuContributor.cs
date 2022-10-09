using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Full.Abp.CategoryManagement.Web.Menus;

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
        //Add main menu items.
        context.Menu.AddItem(new ApplicationMenuItem(CategoryManagementMenus.Prefix, displayName: "CategoryManagement", "~/CategoryManagement", icon: "fa fa-globe"));

        return Task.CompletedTask;
    }
}
