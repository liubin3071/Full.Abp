using Full.Abp.FeatureManagement.Blazor.Server.AntDesignUI;
using Full.Abp.TenantManagement.Blazor.AntDesignUI;
using Volo.Abp.Modularity;

namespace Full.Abp.TenantManagement.Blazor.Server.AntDesignUI;

[DependsOn(
    typeof(AbpTenantManagementBlazorAntDesignModule),
    typeof(AbpFeatureManagementBlazorWebServerAntDesignModule)
)]
public class AbpTenantManagementBlazorServerAntDesignModule : AbpModule
{
    
}
