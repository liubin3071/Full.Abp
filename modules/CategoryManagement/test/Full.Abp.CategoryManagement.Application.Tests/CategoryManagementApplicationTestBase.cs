using Xunit.Abstractions;

namespace Full.Abp.CategoryManagement;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class CategoryManagementApplicationTestBase : CategoryManagementTestBase<CategoryManagementApplicationTestModule>
{
 protected CategoryManagementApplicationTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
 {
 }
}
