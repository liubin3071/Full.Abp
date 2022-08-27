using Localization.Resources.AbpUi;
using Full.Abp.PaymentManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Full.Abp.PaymentManagement;

[DependsOn(
    typeof(PaymentManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class PaymentManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(PaymentManagementHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<PaymentManagementResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
