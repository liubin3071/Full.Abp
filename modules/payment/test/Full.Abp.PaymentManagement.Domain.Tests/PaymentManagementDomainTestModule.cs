using Full.Abp.PaymentManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Full.Abp.PaymentManagement;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(PaymentManagementEntityFrameworkCoreTestModule)
    )]
public class PaymentManagementDomainTestModule : AbpModule
{

}
