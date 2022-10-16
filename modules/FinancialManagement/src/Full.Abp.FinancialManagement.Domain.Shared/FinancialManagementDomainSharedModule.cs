using Full.Abp.FinancialManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Full.Abp.FinancialManagement;

[DependsOn(
    typeof(AbpValidationModule)
)]
public class FinancialManagementDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<FinancialManagementDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<FinancialManagementResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/FinancialManagement");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("FinancialManagement", typeof(FinancialManagementResource));
        });
    }
}
