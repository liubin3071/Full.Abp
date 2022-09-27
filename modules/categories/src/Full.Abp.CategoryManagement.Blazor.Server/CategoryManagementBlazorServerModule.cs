using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace Full.Abp.CategoryManagement.Blazor.Server;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerThemingModule),
    typeof(CategoryManagementBlazorModule)
    )]
public class CategoryManagementBlazorServerModule : AbpModule
{

}
