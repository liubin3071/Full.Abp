using Full.Abp.Categories;
using Volo.Abp.DependencyInjection;

namespace Full.Abp.CategoryManagement;

public class CategoryServiceFactory : ICategoryServiceFactory, ITransientDependency
{
    private readonly IAbpLazyServiceProvider _lazyServiceProvider;

    public CategoryServiceFactory(IAbpLazyServiceProvider lazyServiceProvider)
    {
        _lazyServiceProvider = lazyServiceProvider;
    }

    public ICategoryService Create(string providerName, string providerKey)
    {
        return new CategoryService(_lazyServiceProvider, providerName, providerKey);
    }
}