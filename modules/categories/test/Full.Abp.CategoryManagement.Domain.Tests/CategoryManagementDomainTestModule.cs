using Full.Abp.CategoryManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Full.Abp.CategoryManagement;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(CategoryManagementEntityFrameworkCoreTestModule)
    )]
public class CategoryManagementDomainTestModule : AbpModule
{

}
