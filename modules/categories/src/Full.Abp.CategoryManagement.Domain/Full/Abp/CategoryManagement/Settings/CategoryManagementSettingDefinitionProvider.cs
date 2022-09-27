using Volo.Abp.Settings;

namespace Full.Abp.CategoryManagement.Settings;

public class CategoryManagementSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        /* Define module settings here.
         * Use names from CategoryManagementSettings class.
         */
        context.Add();
    }
}
