using Full.Abp.FinancialManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Full.Abp.FinancialManagement;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(FinancialManagementEntityFrameworkCoreTestModule)
    )]
public class FinancialManagementDomainTestModule : AbpModule
{

}
