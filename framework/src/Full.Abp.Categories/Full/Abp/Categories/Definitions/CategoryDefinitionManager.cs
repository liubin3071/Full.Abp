using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Full.Abp.Categories.Definitions;

public class CategoryDefinitionManager : ICategoryDefinitionManager, ISingletonDependency
{
    protected Lazy<Dictionary<string, CategoryDefinition>> Definitions { get; }

    private readonly IServiceProvider _serviceProvider;
    protected AbpCategoriesOptions Options { get; }


    public CategoryDefinitionManager(IOptions<AbpCategoriesOptions> options, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Options = options.Value;

        Definitions = new Lazy<Dictionary<string, CategoryDefinition>>(
            CreateDefinitions,
            isThreadSafe: true
        );
    }

    public CategoryDefinition Get(string name)
    {
        var accountDefinition = GetOrNull(name);

        if (accountDefinition == null)
        {
            throw new AbpException("Undefined category: " + name);
        }

        return accountDefinition;
    }

    public CategoryDefinition? GetOrNull(string name)
    {
        Check.NotNull(name, nameof(name));

        return Definitions.Value.GetOrDefault(name);
    }
    
    public IEnumerable<CategoryDefinition> GetAll()
    {
        return Definitions.Value.Values.ToImmutableList();
    }

    protected virtual Dictionary<string, CategoryDefinition> CreateDefinitions()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = new CategoryDefinitionContext(scope.ServiceProvider);

        var providers = Options
            .DefinitionProviders
            .Select(p => scope.ServiceProvider.GetRequiredService(p) as ICategoryDefinitionProvider)
            .ToList();

        foreach (var provider in providers)
        {
            provider!.Define(context);
        }

        return context.Definitions;
    }
}

