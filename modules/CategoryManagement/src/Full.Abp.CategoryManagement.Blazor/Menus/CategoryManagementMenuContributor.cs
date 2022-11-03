using System.Linq;
using System.Threading.Tasks;
using Full.Abp.Categories.Definitions;
using Full.Abp.CategoryManagement.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.MultiTenancy;
using Volo.Abp.UI.Navigation;

namespace Full.Abp.CategoryManagement.Blazor.Menus;

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

    private   Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
         // Add main menu items.
         var categoryDefinitionManager = context.ServiceProvider.GetRequiredService<ICategoryDefinitionManager>();
        //if(categoryDefinitionManager)
         
         var l = context.GetLocalizer<CategoryManagementResource>();
         var categoryMenu = new ApplicationMenuItem(CategoryManagementMenus.Prefix, displayName: l["CategoryManagement"],
             icon: "fa fa-globe");
         var administration = context.Menu.GetAdministration();
         administration.AddItem(categoryMenu);
        
         var currentTenant = context.ServiceProvider.GetService<ICurrentTenant>();
         var multiTenancySide = currentTenant.GetMultiTenancySide();
        
         // var categoryDefinitionManager = context.ServiceProvider.GetRequiredService<ICategoryDefinitionManager>();
        
         foreach (var categoryDefinition in   categoryDefinitionManager.GetAll()
                      .Where(c => c.MultiTenancySides.HasFlag(multiTenancySide)))
         {
             categoryMenu.AddItem(new ApplicationMenuItem(categoryDefinition.Name,
                 categoryDefinition.DisplayName.Localize(context.StringLocalizerFactory), url: $"/CategoryManagement/{categoryDefinition.Name}",
                 requiredPermissionName: $"CategoryManagement.{categoryDefinition.Name}"));
         }

         return Task.CompletedTask;
    }
    
    
}