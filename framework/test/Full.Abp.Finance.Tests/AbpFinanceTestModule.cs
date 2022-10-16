using Volo.Abp.Modularity;

namespace Full.Abp.Finance.Test;

[DependsOn(typeof(AbpFinanceModule))]
public class AbpFinanceTestModule : AbpModule
{
    
}