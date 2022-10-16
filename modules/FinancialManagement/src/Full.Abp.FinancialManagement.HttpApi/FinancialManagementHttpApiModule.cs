using Full.Abp.FinancialManagement.Localization;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Full.Abp.FinancialManagement;

[DependsOn(
    typeof(FinancialManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule)
)]
public class FinancialManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(FinancialManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<FinancialManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}