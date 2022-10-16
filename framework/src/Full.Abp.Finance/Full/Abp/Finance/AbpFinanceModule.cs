using Full.Abp.Finance.Accounts;
using Full.Abp.Finance.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Security;
using Volo.Abp.VirtualFileSystem;

namespace Full.Abp.Finance;

[DependsOn(
    typeof(AbpFinanceAbstractionsModule),
    typeof(AbpSecurityModule),
    typeof(AbpLocalizationModule)
)]
public class AbpFinanceModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddDefinitionProviders(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options => { options.FileSets.AddEmbedded<AbpFinanceResource>(); });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<AbpFinanceResource>("en")
                .AddVirtualJson("/Full/Abp/Finance/Localization");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Full.Finance", typeof(AbpFinanceResource));
        });
    }

    private static void AutoAddDefinitionProviders(IServiceCollection services)
    {
        var definitionProviders = new List<Type>();

        services.OnRegistred(context =>
        {
            if (typeof(IAccountDefinitionProvider).IsAssignableFrom(context.ImplementationType))
            {
                definitionProviders.Add(context.ImplementationType);
            }
        });

        services.Configure<AbpFinanceOptions>(options =>
        {
            options.AccountDefinitionProviders.AddIfNotContains(definitionProviders);
        });
    }
}