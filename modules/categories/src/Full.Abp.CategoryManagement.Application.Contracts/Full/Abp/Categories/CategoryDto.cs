using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Full.Abp.Categories;

public class CategoryDto : IEntityDto<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Sequence { get; set; }
    public Guid ParentId { get; set; }
    public IEnumerable<CategoryDto> Children { get; set; }
}
