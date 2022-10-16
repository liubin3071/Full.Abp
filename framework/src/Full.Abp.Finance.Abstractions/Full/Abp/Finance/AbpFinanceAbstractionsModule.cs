using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Full.Abp.Finance;

[DependsOn(
    typeof(AbpMultiTenancyModule)
)]
public class AbpFinanceAbstractionsModule : AbpModule
{
}