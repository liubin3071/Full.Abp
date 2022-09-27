using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Full.Abp.Categories;

public interface ICategoryAppService : IApplicationService
{
    Task<Guid> GetTreeIdAsync(string definitionName);
    Task<ListResultDto<CategoryDto>> GetAncestorsAsync(Guid id);
    Task<CategoryDto?> GetAncestorAsync(GetAncestorInput input);
    Task<ListResultDto<CategoryDto>> GetAllAsync(string definitionName);
    Task<List<CategoryDto>> GetTreeAsync(string definitionName);
    Task<ListResultDto<CategoryDto>> GetDescendantsAsync(CategoryGetDescendantsInput input);
    Task<bool> HasChildrenAsync(Guid id);
    Task<int> GetChildrenCountAsync(Guid id);
    Task<ListResultDto<CategoryDto>> GetChildrenAsync(Guid id);
    Task<PagedResultDto<CategoryDto>> GetPagedChildrenAsync(CategoryGetChildrenInput input);
    Task<CategoryDto> CreateAsync(CategoryCreateInput input);
    Task<CategoryDto> UpdateAsync(Guid id, CategoryUpdateInput input);
    Task DeleteAsync(Guid id);
    Task DeleteManyAsync(IEnumerable<Guid> ids);
    Task<CategoryDto> GetAsync(Guid entityId);
}