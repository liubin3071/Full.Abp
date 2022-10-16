using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Full.Abp.Finance.Accounts;

public class AccountDefinitionContext : IAccountDefinitionContext
{
    public IServiceProvider ServiceProvider { get; }

    public Dictionary<string, AccountDefinition> AccountDefinitions { get; }

    public AccountDefinitionContext(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        AccountDefinitions = new Dictionary<string, AccountDefinition>();
    }

    /// <summary>
    /// Tries to get a pre-defined permission group.
    /// Returns null if can not find the given group.
    /// <param name="name">Name of the group</param>
    /// </summary>
    public AccountDefinition? GetAccountOrNull(string name)
    {
        Check.NotNull(name, nameof(name));
        return AccountDefinitions.GetOrDefault(name);
    }

    public AccountDefinition GetAccount(string name)
    {
        Check.NotNull(name, nameof(name));
        return AccountDefinitions[name];
    }

    public AccountDefinition AddAccount(string name, ILocalizableString? displayName = null, int precision = 2,
        MultiTenancySides multiTenancySides = MultiTenancySides.Both)
    {
        Check.NotNull(name, nameof(name));
        var account = new AccountDefinition(name, displayName, precision, multiTenancySides);
        AccountDefinitions.Add(name, account);
        return account;
    }
}