using Full.Abp.Finance;
using Full.Abp.FinancialManagement.Blazor.Menus;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Full.Abp.FinancialManagement.Blazor;

[DependsOn(
    typeof(FinancialManagementApplicationContractsModule),
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpFinanceModule)
    )]
public class FinancialManagementBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<FinancialManagementBlazorModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<FinancialManagementBlazorAutoMapperProfile>(validate: true);
        });

        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new FinancialManagementMenuContributor());
        });

        Configure<AbpRouterOptions>(options =>
        {
            options.AdditionalAssemblies.Add(typeof(FinancialManagementBlazorModule).Assembly);
        });
    }
}
