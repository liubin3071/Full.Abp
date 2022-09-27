using Full.Abp.Categories.Definitions;
using Full.Abp.CategoryManagement.Localization;
using Volo.Abp.Localization;

namespace Full.Abp.CategoryDemo.Blazor.Server;

public class TestCategoryDefinitionProvider : CategoryDefinitionProvider
{
    public override void Define(ICategoryDefinitionContext context)
    {
        context.Add(new CategoryDefinition("DemoCat","Category:DemoCat"));
    }
    
    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CategoryManagementResource>(name);
    }
}