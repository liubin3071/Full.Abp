using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Full.Abp.CategoryManagement;

public class CategoryDto : IEntityDto<Guid>,ITreeNode<CategoryDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Sequence { get; set; }
    public CategoryDto? Parent { get; set; }
    public IEnumerable<CategoryDto> Children { get; set; }
}
