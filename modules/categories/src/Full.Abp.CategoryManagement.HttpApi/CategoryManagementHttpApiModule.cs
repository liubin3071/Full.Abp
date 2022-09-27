using Full.Abp.Categories;
using Localization.Resources.AbpUi;
using Full.Abp.CategoryManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Full.Abp.CategoryManagement;

[DependsOn(
    typeof(CategoryManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class CategoryManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CategoryManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<CategoryManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
