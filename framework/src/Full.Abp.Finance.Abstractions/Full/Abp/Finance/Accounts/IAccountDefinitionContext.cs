using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Full.Abp.Finance.Accounts;

public interface IAccountDefinitionContext
{
    IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Tries to get a pre-defined permission group.
    /// Returns null if can not find the given group.
    /// <param name="name">Name of the group</param>
    /// </summary>
    AccountDefinition? GetAccountOrNull(string name);

    AccountDefinition GetAccount(string name);

    AccountDefinition AddAccount(string name, ILocalizableString? displayName = null, int rate = 2,
        MultiTenancySides multiTenancySides = MultiTenancySides.Both);
}