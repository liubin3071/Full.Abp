using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Full.Abp.FinancialManagement.Blazor.WebAssembly;

[DependsOn(
    typeof(FinancialManagementBlazorModule),
    typeof(FinancialManagementHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
    )]
public class FinancialManagementBlazorWebAssemblyModule : AbpModule
{

}
