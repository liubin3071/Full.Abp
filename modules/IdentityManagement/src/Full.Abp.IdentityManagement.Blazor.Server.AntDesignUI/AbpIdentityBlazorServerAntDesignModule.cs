using Full.Abp.IdentityManagement.Blazor.AntDesignUI;
using Full.Abp.PermissionManagement.Blazor.AntDesignUI;
using Volo.Abp.Modularity;

namespace Full.Abp.IdentityManagement.Blazor.Server.AntDesignUI;

[DependsOn(
    typeof(AbpIdentityBlazorAntDesignModule),
    typeof(AbpPermissionManagementBlazorAntDesignModule)
)]
public class AbpIdentityBlazorServerAntDesignModule : AbpModule
{
    
}
