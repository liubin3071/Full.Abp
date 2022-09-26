using Full.Abp.FeatureManagement.Blazor.WebAssembly.AntDesignUI;
using Full.Abp.TenantManagement.Blazor.AntDesignUI;
using Volo.Abp.Modularity;
using Volo.Abp.TenantManagement;

namespace Full.Abp.TenantManagement.Blazor.WebAssembly.AntDesignUI;


[DependsOn(
    typeof(AbpTenantManagementBlazorAntDesignModule),
    typeof(AbpFeatureManagementBlazorWebAssemblyAntDesignModule),
    typeof(AbpTenantManagementHttpApiClientModule)
)]
public class AbpTenantManagementBlazorWebAssemblyAntDesignModule : AbpModule
{
    
}
