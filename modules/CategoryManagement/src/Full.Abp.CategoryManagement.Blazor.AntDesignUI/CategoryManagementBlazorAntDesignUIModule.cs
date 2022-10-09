using Full.Abp.AspnetCore.Components.Web.AntDesignTheme;
using Full.Abp.AspnetCore.Components.Web.AntDesignTheme.Routing;
using Full.Abp.Categories;
using Full.Abp.CategoryManagement.Blazor.AntDesignUI.Menus;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Full.Abp.CategoryManagement.Blazor.AntDesignUI;

[DependsOn(
    typeof(CategoryManagementApplicationContractsModule),
    typeof(AbpAspNetCoreComponentsWebAntDesignThemeModule),
    typeof(AbpAutoMapperModule)
    )]
public class CategoryManagementBlazorAntDesignUiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<CategoryManagementBlazorAntDesignUiModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<CategoryManagementBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new CategoryManagementMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(CategoryManagementBlazorAntDesignUiModule).Assembly);
        });
    }
}
