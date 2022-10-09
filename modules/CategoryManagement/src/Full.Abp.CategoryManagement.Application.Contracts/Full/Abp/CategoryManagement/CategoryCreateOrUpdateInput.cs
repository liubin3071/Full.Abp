using System;

namespace Full.Abp.CategoryManagement;

public class CategoryCreateOrUpdateInput
{
    public string Name { get; set; } = null!;
    public int Sequence { get; set; }
    public Guid? ParentId { get; set; }
}