using System;
using System.Collections.Generic;

namespace Full.Abp.Categories;

public class CategoryKeys
{
    public string ProviderName { get; set; } = null!;
    public string? ProviderKey { get; set; }
    public IEnumerable<Guid> Ids { get; set; }
}