using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Full.Abp.Trees;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(TreesDomainSharedModule),
    typeof(AbpTreesModule)
)]
public class TreesDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient(typeof(ITreeEntityServiceFactory<,,>), typeof(TreeEntityServiceFactory<,,>));
    }
}
