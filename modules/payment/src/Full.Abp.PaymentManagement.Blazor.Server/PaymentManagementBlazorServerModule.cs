using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace Full.Abp.PaymentManagement.Blazor.Server;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerThemingModule),
    typeof(PaymentManagementBlazorModule)
    )]
public class PaymentManagementBlazorServerModule : AbpModule
{

}
