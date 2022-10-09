using System;
using Volo.Abp.Application.Dtos;

namespace Full.Abp.CategoryManagement;

public class CategoryGetChildrenInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
}