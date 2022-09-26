using Full.Abp.IdentityManagement.Blazor.AntDesignUI;
using Full.Abp.PermissionManagement.Blazor.WebAssembly.AntDesignUI;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Full.Abp.IdentityManagement.Blazor.WebAssembly.AntDesignUI;

[DependsOn(
    typeof(AbpIdentityBlazorAntDesignModule),
    typeof(AbpPermissionManagementBlazorWebAssemblyAntDesignModule),
    typeof(AbpIdentityHttpApiClientModule)
)]
public class AbpIdentityBlazorWebAssemblyAntDesignModule: AbpModule
{
}
