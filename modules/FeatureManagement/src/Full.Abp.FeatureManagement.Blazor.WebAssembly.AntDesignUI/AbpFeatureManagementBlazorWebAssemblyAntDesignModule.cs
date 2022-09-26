using Full.Abp.AspnetCore.Components.WebAssembly.AntDesignTheme;
using Full.Abp.FeatureManagement.Blazor.AntDesignUI;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;

namespace Full.Abp.FeatureManagement.Blazor.WebAssembly.AntDesignUI;

[DependsOn(
    typeof(AbpFeatureManagementBlazorAntDesignModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyAntDesignThemeModule),
    typeof(AbpFeatureManagementHttpApiClientModule)
)]
public class AbpFeatureManagementBlazorWebAssemblyAntDesignModule : AbpModule
{
}
