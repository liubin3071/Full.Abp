using Full.Abp.AspnetCore.Components.WebAssembly.AntDesignTheme;
using Full.Abp.PermissionManagement.Blazor.AntDesignUI;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace Full.Abp.PermissionManagement.Blazor.WebAssembly.AntDesignUI;

[DependsOn(
    typeof(AbpPermissionManagementBlazorAntDesignModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyAntDesignThemeModule),
    typeof(AbpPermissionManagementHttpApiClientModule)
)]
public class AbpPermissionManagementBlazorWebAssemblyAntDesignModule : AbpModule
{
}
