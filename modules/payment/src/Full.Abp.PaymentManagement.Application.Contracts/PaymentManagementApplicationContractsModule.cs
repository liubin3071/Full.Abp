using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Full.Abp.PaymentManagement;

[DependsOn(
    typeof(PaymentManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class PaymentManagementApplicationContractsModule : AbpModule
{

}
