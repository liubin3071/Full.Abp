using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Full.Abp.FinancialManagement;

[DependsOn(
    typeof(FinancialManagementDomainModule),
    typeof(FinancialManagementApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
)]
public class FinancialManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<FinancialManagementApplicationModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<FinancialManagementApplicationModule>(validate: true); });
    }
}