using System;

namespace Full.Abp.CategoryManagement;

public class GetAncestorInput
{
    public Guid Id { get; set; }

    public int Deepin { get; set; }
}