using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Full.Abp.Finance.Accounts;

public class AccountDefinitionManager : IAccountDefinitionManager,ISingletonDependency
{
    protected IDictionary<string, AccountDefinition> AccountDefinitions => _lazyAccountDefinitions.Value;
    private readonly Lazy<Dictionary<string, AccountDefinition>> _lazyAccountDefinitions;

    private readonly IServiceProvider _serviceProvider;
    protected AbpFinanceOptions Options { get; }

    public AccountDefinitionManager(
        IOptions<AbpFinanceOptions> options,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Options = options.Value;

        _lazyAccountDefinitions = new Lazy<Dictionary<string, AccountDefinition>>(
            CreateAccountDefinitions,
            isThreadSafe: true
        );
    }

    public AccountDefinition Get(string name)
    {
        var accountDefinition = GetOrNull(name);

        if (accountDefinition == null)
        {
            throw new AbpException("Undefined account: " + name);
        }

        return accountDefinition;
    }

    public AccountDefinition GetOrNull(string name)
    {
        Check.NotNull(name, nameof(name));

        return AccountDefinitions.GetOrDefault(name);
    }

    public IReadOnlyList<AccountDefinition> GetAll()
    {
        return AccountDefinitions.Values.ToImmutableList();
    }

    protected virtual Dictionary<string, AccountDefinition> CreateAccountDefinitions()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = new AccountDefinitionContext(scope.ServiceProvider);

        var providers = Options
            .AccountDefinitionProviders
            .Select(p => scope.ServiceProvider.GetRequiredService(p) as IAccountDefinitionProvider)
            .ToList();

        foreach (var provider in providers)
        {
            provider!.PreDefine(context);
        }

        foreach (var provider in providers)
        {
            provider!.Define(context);
        }

        foreach (var provider in providers)
        {
            provider!.PostDefine(context);
        }

        return context.AccountDefinitions;
    }
}