using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Full.Abp.Trees;

[Serializable]
public class Tree<TKey> : Entity<TKey>,IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string ProviderType { get; set; }

    public string ProviderName { get; set; }

    public string ProviderKey { get; set; }
}