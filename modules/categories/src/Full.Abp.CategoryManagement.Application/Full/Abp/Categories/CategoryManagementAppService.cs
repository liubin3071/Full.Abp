using Full.Abp.CategoryManagement.Localization;
using Volo.Abp.Application.Services;

namespace Full.Abp.CategoryManagement.Full.Abp.Categories;

public abstract class CategoryManagementAppService : ApplicationService
{
    protected CategoryManagementAppService()
    {
        LocalizationResource = typeof(CategoryManagementResource);
        ObjectMapperContext = typeof(CategoryManagementApplicationModule);
    }
}
