using Full.Abp.AspnetCore.Components.Server.AntDesignTheme;
using Full.Abp.FeatureManagement.Blazor.AntDesignUI;
using Volo.Abp.Modularity;

namespace Full.Abp.FeatureManagement.Blazor.Server.AntDesignUI;

[DependsOn(
    typeof(AbpFeatureManagementBlazorAntDesignModule),
    typeof(AbpAspNetCoreComponentsServerAntDesignThemeModule)
)]
public class AbpFeatureManagementBlazorWebServerAntDesignModule : AbpModule
{
}
