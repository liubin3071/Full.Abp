using Volo.Abp.Collections;

namespace Full.Abp.Categories.Definitions;

public class AbpCategoriesOptions
{
    public ITypeList<ICategoryDefinitionProvider> DefinitionProviders { get; }
    public ITypeList<ICategoryService>  Providers { get; }

    public AbpCategoriesOptions()
    {
        DefinitionProviders = new TypeList<ICategoryDefinitionProvider>();
        Providers = new TypeList<ICategoryService>();
    }
}