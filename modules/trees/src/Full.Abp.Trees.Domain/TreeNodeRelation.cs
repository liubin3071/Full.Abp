using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Full.Abp.Trees;

[Serializable]
public abstract class TreeNodeRelation<TKey> : Entity, IMultiTenant, IHasConcurrencyStamp
{
    public Guid? TenantId { get; set; }
    public TKey Ancestor { get; set; }
    public TKey Descendant { get; set; }
    public int Distance { get; set; }
    public TKey TreeId { get; set; }

    public override object[] GetKeys()
    {
        return new object[] { Ancestor, Descendant, TreeId };
    }

    public string ConcurrencyStamp { get; set; }
}