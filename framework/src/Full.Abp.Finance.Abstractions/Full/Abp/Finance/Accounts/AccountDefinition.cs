using System.Collections.Immutable;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SimpleStateChecking;

namespace Full.Abp.Finance.Accounts;

/// <summary>
/// 账户定义. 提供显示名称，货币符号等信息
/// </summary>
public class AccountDefinition : IHasSimpleStateCheckers<AccountDefinition>
{
    /// <summary>
    /// Unique name of the permission.
    /// </summary>
    public string Name { get; }

    public ILocalizableString DisplayName {
        get => _displayName;
        set => _displayName = Check.NotNull(value, nameof(value));
    }

    private ILocalizableString _displayName;

    public int Precision { get; }

    /// <summary>
    /// MultiTenancy side.
    /// Default: <see cref="MultiTenancySides.Both"/>
    /// </summary>
    public MultiTenancySides MultiTenancySide { get; set; }

    /// <summary>
    /// A list of allowed providers to get/set value of this permission.
    /// An empty list indicates that all providers are allowed.
    /// </summary>
    public List<string> AllowedProviders { get; }

    public List<ISimpleStateChecker<AccountDefinition>> StateCheckers { get; }

    public object this[string name] {
        get => Properties.GetOrDefault(name);
        set => Properties[name] = value;
    }

    public Dictionary<string, object> Properties { get; }

    protected internal AccountDefinition(
        string name,
        ILocalizableString? displayName = null, int precision = 2,
        MultiTenancySides multiTenancySide = MultiTenancySides.Both
    )
    {
        Name = Check.NotNull(name, nameof(name));
        DisplayName = displayName ?? new FixedLocalizableString(name);
        Precision = precision;
        MultiTenancySide = multiTenancySide;

        Properties = new Dictionary<string, object>();
        AllowedProviders = new List<string>();
        StateCheckers = new List<ISimpleStateChecker<AccountDefinition>>();
    }

    /// <summary>
    /// Sets a property in the <see cref="Properties"/> dictionary.
    /// This is a shortcut for nested calls on this object.
    /// </summary>
    public virtual AccountDefinition WithProperty(string key, object value)
    {
        Properties[key] = value;
        return this;
    }

    public bool IsAllowedProvider(string providerName)
    {
        return AllowedProviders.IsNullOrEmpty() || AllowedProviders.Contains(providerName);
    }
 
}