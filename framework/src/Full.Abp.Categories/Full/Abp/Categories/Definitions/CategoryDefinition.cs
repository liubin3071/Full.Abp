using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Full.Abp.Categories.Definitions;

public class CategoryDefinition
{
    /// <summary>
    /// Unique name of the permission.
    /// </summary>
    public string Name { get; }
    
    public MultiTenancySides MultiTenancySides { get; set; }

    private ILocalizableString _displayName;
    public ILocalizableString DisplayName {
        get => _displayName;
        set => _displayName = Check.NotNull(value, nameof(value));
    }
    
    public object this[string name] {
        get => Properties.GetOrDefault(name);
        set => Properties[name] = value;
    }

    public Dictionary<string, object> Properties { get; }

    public virtual CategoryDefinition WithProperty(string key, object value)
    {
        Properties[key] = value;
        return this;
    }

    public CategoryDefinition(string name,
        ILocalizableString? displayName = null,
        MultiTenancySides multiTenancySide = MultiTenancySides.Both
    )
    {
        Name = Check.NotNull(name, nameof(name));
        DisplayName = displayName ?? new FixedLocalizableString(name);
        MultiTenancySides = multiTenancySide;
        Properties = new Dictionary<string, object>();
    }
}