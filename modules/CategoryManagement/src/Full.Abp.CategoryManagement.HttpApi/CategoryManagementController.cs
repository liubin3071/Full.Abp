using Full.Abp.CategoryManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Full.Abp.CategoryManagement;

public abstract class CategoryManagementController : AbpControllerBase
{
    protected CategoryManagementController()
    {
        LocalizationResource = typeof(CategoryManagementResource);
    }
}
