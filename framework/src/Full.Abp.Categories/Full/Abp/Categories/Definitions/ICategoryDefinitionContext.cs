namespace Full.Abp.Categories.Definitions;

public interface ICategoryDefinitionContext
{
    IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Tries to get a pre-defined permission group.
    /// Returns null if can not find the given group.
    /// <param name="name">Name of the group</param>
    /// </summary>
    CategoryDefinition? GetOrNull(string name);

    CategoryDefinition Get(string name);

    void Add(params CategoryDefinition[] definitions);
}