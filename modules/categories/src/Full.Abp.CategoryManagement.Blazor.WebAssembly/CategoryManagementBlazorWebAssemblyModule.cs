using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Full.Abp.CategoryManagement.Blazor.WebAssembly;

[DependsOn(
    typeof(CategoryManagementBlazorModule),
    typeof(CategoryManagementHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
    )]
public class CategoryManagementBlazorWebAssemblyModule : AbpModule
{

}
