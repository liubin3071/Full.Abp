using Xunit.Abstractions;

namespace Full.Abp.CategoryManagement;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class CategoryManagementDomainTestBase : CategoryManagementTestBase<CategoryManagementDomainTestModule>
{
 protected CategoryManagementDomainTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
 {
 }
}
