using System;
using Full.Abp.Trees;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace Full.Abp.CategoryManagement;

public class Category : AggregateRoot<Guid>, IMultiTenant
{
    public string Name { get; set; } = null!;
    public int Sequence { get; set; }
    
    public Guid? TenantId { get; set; }
}