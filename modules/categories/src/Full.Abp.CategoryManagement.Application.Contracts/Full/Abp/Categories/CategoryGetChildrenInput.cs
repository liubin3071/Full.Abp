using System;
using Volo.Abp.Application.Dtos;

namespace Full.Abp.Categories;

public class CategoryGetChildrenInput : PagedAndSortedResultRequestDto
{
    public Guid Id { get; set; }
    public string? Filter { get; set; }
}