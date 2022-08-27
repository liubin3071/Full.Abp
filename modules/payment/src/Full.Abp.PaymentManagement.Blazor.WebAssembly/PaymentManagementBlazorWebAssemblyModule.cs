using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Full.Abp.PaymentManagement.Blazor.WebAssembly;

[DependsOn(
    typeof(PaymentManagementBlazorModule),
    typeof(PaymentManagementHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
    )]
public class PaymentManagementBlazorWebAssemblyModule : AbpModule
{

}
