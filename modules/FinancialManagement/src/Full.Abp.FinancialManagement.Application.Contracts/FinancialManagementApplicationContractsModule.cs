using Full.Abp.Finance;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Full.Abp.FinancialManagement;

[DependsOn(
    typeof(FinancialManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpFinanceModule)
)]
public class FinancialManagementApplicationContractsModule : AbpModule
{
}