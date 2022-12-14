using System.Linq;
using System.Threading.Tasks;
using Full.Abp.Categories.Definitions;
using Full.Abp.CategoryManagement.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.UI.Navigation;

namespace Full.Abp.CategoryManagement.Blazor.AntDesignUI.Menus;

public class CategoryManagementMenuContributor : IMenuContributor
{

    public CategoryManagementMenuContributor()
    {
    }

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
        var categoryMenu = new ApplicationMenuItem(CategoryManagementMenus.Prefix, displayName: "CategoryManagement",
            icon: "fa fa-globe");
        var administration = context.Menu.GetAdministration();
        administration.AddItem(categoryMenu);

        var currentTenant = context.ServiceProvider.GetService<ICurrentTenant>();
        var multiTenancySide = currentTenant.GetMultiTenancySide();

        var categoryDefinitionManager = context.ServiceProvider.GetRequiredService<ICategoryDefinitionManager>();
        var l = context.GetLocalizer<CategoryManagementResource>();
        
        foreach (var categoryDefinition in categoryDefinitionManager.GetAll()
                     .Where(c => c.MultiTenancySides.HasFlag(multiTenancySide)))
        {
            // categoryDefinition.DisplayName.Localize(l);
            categoryMenu.AddItem(new ApplicationMenuItem(categoryDefinition.Name,
                l[categoryDefinition.Name], url: $"/CategoryManagement/{categoryDefinition.Name}",
                requiredPermissionName: $"CategoryManagement.{categoryDefinition.Name}"));
        }

        return Task.CompletedTask;
    }
}