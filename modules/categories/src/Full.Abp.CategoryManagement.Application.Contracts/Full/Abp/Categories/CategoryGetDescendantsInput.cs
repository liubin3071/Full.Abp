using System;

namespace Full.Abp.Categories;

 

public class CategoryGetDescendantsInput
{
    public Guid Id { get; set; }
    public int? MaxDistance { get; set; }
}