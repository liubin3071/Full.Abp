using System;

namespace Full.Abp.Categories;

public class GetAncestorInput
{
    public Guid Id { get; set; }

    public int Deepin { get; set; }
}