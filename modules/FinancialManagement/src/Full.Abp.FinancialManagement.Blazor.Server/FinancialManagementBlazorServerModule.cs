using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace Full.Abp.FinancialManagement.Blazor.Server;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerThemingModule),
    typeof(FinancialManagementBlazorModule)
    )]
public class FinancialManagementBlazorServerModule : AbpModule
{

}
