using Full.Abp.AspnetCore.Components.Server.AntDesignTheme;
using Volo.Abp.Modularity;
using Full.Abp.SettingManagement.Blazor.AntDesignUI;

namespace Full.Abp.SettingManagement.Blazor.Server.AntDesignUI;

[DependsOn(
    typeof(AbpSettingManagementBlazorAntDesignModule),
    typeof(AbpAspNetCoreComponentsServerAntDesignThemeModule)
)]
public class AbpSettingManagementBlazorServerAntDesignModule : AbpModule
{
    
}
