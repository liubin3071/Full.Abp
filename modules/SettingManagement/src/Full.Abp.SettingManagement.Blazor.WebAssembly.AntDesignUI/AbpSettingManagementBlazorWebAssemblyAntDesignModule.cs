using Full.Abp.AspnetCore.Components.WebAssembly.AntDesignTheme;
using Volo.Abp.Modularity;
using Full.Abp.SettingManagement.Blazor.AntDesignUI;
using Volo.Abp.SettingManagement;

namespace Full.Abp.SettingManagement.Blazor.WebAssembly.AntDesignUI;

[DependsOn(
    typeof(AbpSettingManagementBlazorAntDesignModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyAntDesignThemeModule),
    typeof(AbpSettingManagementHttpApiClientModule)
)]
public class AbpSettingManagementBlazorWebAssemblyAntDesignModule : AbpModule
{
    
}
