using Volo.Abp.Modularity;

namespace Full.Abp.PaymentManagement;

[DependsOn(
    typeof(PaymentManagementApplicationModule),
    typeof(PaymentManagementDomainTestModule)
    )]
public class PaymentManagementApplicationTestModule : AbpModule
{

}
