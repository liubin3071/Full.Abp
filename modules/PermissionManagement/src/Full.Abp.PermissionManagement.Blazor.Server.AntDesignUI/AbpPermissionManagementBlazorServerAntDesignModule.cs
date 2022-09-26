using Full.Abp.AspnetCore.Components.Server.AntDesignTheme;
using Full.Abp.PermissionManagement.Blazor.AntDesignUI;
using Volo.Abp.Modularity;

namespace Full.Abp.PermissionManagement.Blazor.Server.AntDesignUI;

[DependsOn(
    typeof(AbpPermissionManagementBlazorAntDesignModule),
    typeof(AbpAspNetCoreComponentsServerAntDesignThemeModule)
)]
public class AbpPermissionManagementBlazorServerAntDesignModule : AbpModule
{
}
