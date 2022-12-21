using Volo.Abp.Settings;

namespace Full.Abp.Categories;

public interface ICategoryServiceFactory
{
    ICategoryService Create(string providerName, string providerKey);
}

public static class CategoryServiceFactoryExtensions
{


    
    
}