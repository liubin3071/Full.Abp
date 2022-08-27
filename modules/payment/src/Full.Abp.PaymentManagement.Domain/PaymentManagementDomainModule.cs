using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Full.Abp.PaymentManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(PaymentManagementDomainSharedModule)
)]
public class PaymentManagementDomainModule : AbpModule
{

}
