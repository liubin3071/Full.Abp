using Full.Abp.CategoryManagement.CategoryRelations;
using Xunit.Abstractions;

namespace Full.Abp.CategoryManagement.EntityFrameworkCore.CategoryRelations;

public class CategoryRelationRepository_Tests : CategoryRelationRepository_Tests<CategoryManagementEntityFrameworkCoreTestModule>
{
    /* Don't write custom repository tests here, instead write to
     * the base class.
     * One exception can be some specific tests related to EF core.
     */
    public CategoryRelationRepository_Tests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    {
    }
}
