using Volo.Abp.Guids;
using Volo.Abp.Modularity;

namespace Full.Ids;

[DependsOn(typeof(AbpGuidsModule))]
public class AbpIdModule : AbpModule
{
}