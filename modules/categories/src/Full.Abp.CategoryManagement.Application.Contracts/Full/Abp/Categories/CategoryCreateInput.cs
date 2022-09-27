using System;

namespace Full.Abp.Categories;

public class CategoryCreateInput
{
    public string Name { get; set; } = null!;
    public int Sequence { get; set; }
    public Guid ParentId { get; set; }
}