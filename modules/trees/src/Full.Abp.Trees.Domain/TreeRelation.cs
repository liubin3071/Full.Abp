using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Full.Abp.Trees;

[Serializable]
public abstract class TreeRelation<TKey> : BasicAggregateRoot, IMultiTenant, IHasConcurrencyStamp
{
    public Guid? TenantId { get; set; }
    public TKey Ancestor { get; set; }
    public TKey Descendant { get; set; }
    public int Distance { get; set; }

    public string ProviderType { get; set; }

    public string ProviderName { get; set; }

    public string ProviderKey { get; set; }

    public string ConcurrencyStamp { get; set; }

    public bool IsRoot { get; set; }

    public override object[] GetKeys()
    {
        return new object[] { ProviderType, ProviderName, ProviderKey, Ancestor!, Descendant! };
    }
}