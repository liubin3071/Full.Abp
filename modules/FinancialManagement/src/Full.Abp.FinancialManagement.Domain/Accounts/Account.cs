using Full.Abp.Finance.Accounts;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace Full.Abp.FinancialManagement.Accounts;

public class Account : AggregateRoot<Guid>, IMultiTenant
{
    public Account(string providerName, string providerKey, string name,
        bool isEnabled = true)
    {
        ProviderName = providerName;
        ProviderKey = providerKey;
        Name = name;
        LatestEntryIndex = 0;
        IsEnabled = isEnabled;
    }

    public Guid? TenantId { get; set; }
    public string ProviderName { get; set; }
    public string ProviderKey { get; set; }

    /// <summary>
    /// <see cref="AccountDefinition"/>
    /// </summary>
    public string Name { get; set; }

    public decimal Balance { get; set; }

    public int LatestEntryIndex { get; set; }

    public bool IsEnabled { get; set; }
}