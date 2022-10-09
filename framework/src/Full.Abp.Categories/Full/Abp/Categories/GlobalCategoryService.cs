using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Full.Abp.Categories;

public class GlobalCategoryService : CategoryServiceBase, ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;
    protected override string ProviderKey => string.Empty;
    protected override string ProviderName => "G";

    public GlobalCategoryService(ICategoryServiceFactory categoryServiceFactory, ICurrentTenant currentTenant) 
        : base(categoryServiceFactory)
    {
        _currentTenant = currentTenant;
    }
}