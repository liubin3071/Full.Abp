using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Full.Abp.Categories;

public class TenantOrGlobalCategoryService : CategoryServiceBase, ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;
    protected override string ProviderKey => _currentTenant.ToString();
    protected override string ProviderName => _currentTenant.Id.HasValue ? "T" : "G";

    public TenantOrGlobalCategoryService(ICategoryServiceFactory categoryServiceFactory, ICurrentTenant currentTenant)
        : base(categoryServiceFactory)
    {
        _currentTenant = currentTenant;
    }
}