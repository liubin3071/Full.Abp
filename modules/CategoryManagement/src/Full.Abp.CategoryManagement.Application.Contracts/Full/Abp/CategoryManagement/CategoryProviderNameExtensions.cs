using Volo.Abp.MultiTenancy;

namespace Full.Abp.CategoryManagement;

public static class CategoryProviderNameExtensions
{
    public static string GetCategoryProviderName(this ICurrentTenant currentTenant)
    {
        return currentTenant.Id.HasValue ? "T" : "G";
    }
}