using Xunit.Abstractions;

namespace Full.Abp.CategoryManagement.EntityFrameworkCore;

/* This class can be used as a base class for EF Core integration tests,
 * while SampleRepository_Tests uses a different approach.
 */
public abstract class CategoryManagementEntityFrameworkCoreTestBase : CategoryManagementTestBase<CategoryManagementEntityFrameworkCoreTestModule>
{
 protected CategoryManagementEntityFrameworkCoreTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
 {
 }
}
