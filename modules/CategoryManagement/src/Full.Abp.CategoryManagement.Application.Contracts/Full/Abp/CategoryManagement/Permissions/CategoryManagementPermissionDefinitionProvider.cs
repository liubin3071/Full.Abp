using System.Linq;
using Full.Abp.Categories.Definitions;
using Full.Abp.CategoryManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Full.Abp.CategoryManagement.Permissions;

public class CategoryManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    private readonly ICategoryDefinitionManager _categoryDefinitionManager;

    public CategoryManagementPermissionDefinitionProvider(ICategoryDefinitionManager categoryDefinitionManager)
    {
        _categoryDefinitionManager = categoryDefinitionManager;
    }

    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CategoryManagementPermissions.GroupName, L("Permission:CategoryManagement"));

        var categories = _categoryDefinitionManager.GetAll();
        foreach (var categoryDefinition in categories)
        {
            var permission = CategoryManagementPermissions.Get(categoryDefinition.Name);
            var cat = myGroup.AddPermission(permission.Default,
                L($"Permission:{categoryDefinition.Name}"), categoryDefinition.MultiTenancySides);
            cat.AddChild(permission.Create, L("Permission:Create"), categoryDefinition.MultiTenancySides);
            cat.AddChild(permission.Update, L("Permission:Update"), categoryDefinition.MultiTenancySides);
            cat.AddChild(permission.Delete, L("Permission:Delete"), categoryDefinition.MultiTenancySides);
        }
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CategoryManagementResource>(name);
    }
}