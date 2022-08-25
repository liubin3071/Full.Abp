using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Full.Abp.Identity;

[DependsOn(
    typeof(Volo.Abp.Identity.AbpIdentityDomainModule)
)]
public class AbpIdentityDomainModule : AbpModule
{
}