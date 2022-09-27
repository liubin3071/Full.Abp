using Full.Abp.CategoryManagement.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Full.Abp.CategoryManagement.Pages;

public abstract class CategoryManagementPageModel : AbpPageModel
{
    protected CategoryManagementPageModel()
    {
        LocalizationResourceType = typeof(CategoryManagementResource);
    }
}
