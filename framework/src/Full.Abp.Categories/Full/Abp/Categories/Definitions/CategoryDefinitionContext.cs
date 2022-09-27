using Volo.Abp;

namespace Full.Abp.Categories.Definitions;

public class CategoryDefinitionContext : ICategoryDefinitionContext
{
    public IServiceProvider ServiceProvider { get; }

    public Dictionary<string, CategoryDefinition> Definitions { get; }

    public CategoryDefinitionContext(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        Definitions = new Dictionary<string, CategoryDefinition>();
    }

    /// <summary>
    /// Tries to get a pre-defined permission group.
    /// Returns null if can not find the given group.
    /// <param name="name">Name of the group</param>
    /// </summary>
    public CategoryDefinition? GetOrNull(string name)
    {
        Check.NotNull(name, nameof(name));
        return Definitions.GetOrDefault(name);
    }

    public CategoryDefinition Get(string name)
    {
        Check.NotNull(name, nameof(name));
        return Definitions[name];
    }

    public void Add(params CategoryDefinition[] definitions)
    {
        if (definitions.IsNullOrEmpty())
        {
            return;
        }

        foreach (var definition in definitions)
        {
            Definitions[definition.Name] = definition;
        }
    }
}