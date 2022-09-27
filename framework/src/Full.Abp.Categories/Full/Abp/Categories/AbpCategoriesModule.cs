using Full.Abp.Categories.Definitions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security;

namespace Full.Abp.Categories;

[DependsOn(
    typeof(AbpMultiTenancyModule),
    typeof(AbpLocalizationModule),
    typeof(AbpTreesModule)
)]
public class AbpCategoriesModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AutoAddDefinitionProviders(context.Services);
        // AutoAddProviders(context.Services);
        base.PreConfigureServices(context);
    }

    private static void AutoAddDefinitionProviders(IServiceCollection services)
    {
        var definitionProviders = new List<Type>();

        services.OnRegistred(context =>
        {
            if (typeof(ICategoryDefinitionProvider).IsAssignableFrom(context.ImplementationType))
            {
                definitionProviders.Add(context.ImplementationType);
            }
        });

        services.Configure<AbpCategoriesOptions>(options =>
        {
            options.DefinitionProviders.AddIfNotContains(definitionProviders);
        });
    }
    
    private static void AutoAddProviders(IServiceCollection services)
    {
        var providers = new List<Type>();

        services.OnRegistred(context =>
        {
            if (typeof(ICategoryService).IsAssignableFrom(context.ImplementationType))
            {
                providers.Add(context.ImplementationType);
            }
        });

        services.Configure<AbpCategoriesOptions>(options =>
        {
            options.Providers.AddIfNotContains(providers);
        });
    }
}