using System;

namespace Full.Abp.CategoryManagement;

 

public class CategoryGetDescendantsInput
{
    public Guid Id { get; set; }
    public int? MaxDistance { get; set; }
}