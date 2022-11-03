using Full.Abp.Categories.Definitions;
using Full.Abp.CategoryManagement.Localization;
using Volo.Abp.Localization;

namespace Full.Abp.CategoryManagement;

public class TestCategoryDefinitionProvider : CategoryDefinitionProvider
{
    public override void Define(ICategoryDefinitionContext context)
    {
        context.Add(new CategoryDefinition("Demo",L("Category:DisplayName:Demo")));
    }
    
    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CategoryManagementResource>(name);
    }
}