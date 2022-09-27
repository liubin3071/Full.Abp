namespace Full.Abp.Categories.Definitions;

public interface ICategoryDefinitionManager
{
    CategoryDefinition Get(string name);

    CategoryDefinition GetOrNull(string name);

    IReadOnlyList<CategoryDefinition> GetAll();
}