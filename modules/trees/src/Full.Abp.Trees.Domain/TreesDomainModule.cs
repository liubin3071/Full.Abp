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

}
