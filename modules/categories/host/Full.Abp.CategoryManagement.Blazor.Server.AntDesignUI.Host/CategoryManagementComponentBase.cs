using Full.Abp.CategoryManagement.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Full.Abp.CategoryDemo.Blazor.Server;

public abstract class CategoryManagementComponentBase : AbpComponentBase
{
    protected CategoryManagementComponentBase()
    {
        LocalizationResource = typeof(CategoryManagementResource);
    }
}
