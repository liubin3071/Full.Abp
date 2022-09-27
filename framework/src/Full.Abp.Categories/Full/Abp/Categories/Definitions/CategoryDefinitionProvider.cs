using Volo.Abp.DependencyInjection;

namespace Full.Abp.Categories.Definitions;

public abstract class CategoryDefinitionProvider : ICategoryDefinitionProvider, ITransientDependency
{
    public abstract void Define(ICategoryDefinitionContext context);

}