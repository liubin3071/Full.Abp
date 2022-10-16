using Full.Abp.Finance;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Full.Abp.FinancialManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(FinancialManagementDomainSharedModule),
    typeof(AbpFinanceModule),
    typeof(AbpAutoMapperModule)
)]
public class FinancialManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<FinancialManagementDomainModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<FinancialManagementDomainModule>(validate: true); });
    }
}