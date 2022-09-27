using System;

namespace Full.Abp.Categories;

public class CategoryUpdateInput
{
    public string Name { get; set; } = null!;
    public int Sequence { get; set; }
    public Guid ParentId { get; set; }
}