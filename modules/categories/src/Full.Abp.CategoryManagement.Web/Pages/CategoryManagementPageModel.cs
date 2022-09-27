using Full.Abp.CategoryManagement.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Full.Abp.CategoryManagement.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class CategoryManagementPageModel : AbpPageModel
{
    protected CategoryManagementPageModel()
    {
        LocalizationResourceType = typeof(CategoryManagementResource);
        ObjectMapperContext = typeof(CategoryManagementWebModule);
    }
}
