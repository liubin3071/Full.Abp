using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Full.Abp.CategoryManagement;

public interface ICategoryAppService : IApplicationService
{
    Task<CategoryDto> GetAsync(string definitionName, Guid id);
    Task<ListResultDto<CategoryDto>> GetAncestorsAsync(string definitionName, Guid id);
    Task<CategoryDto?> GetParentAsync(string definitionName, Guid id);
    Task<ListResultDto<CategoryDto>> GetDescendantsAsync(string definitionName, Guid? id, int? maxDistance = null);
    Task<IEnumerable<CategoryDto>> GetTreeAsync(string definitionName);
    Task<bool> HasChildrenAsync(string definitionName, Guid? id);
    Task<int> GetChildrenCountAsync(string definitionName, Guid? id);
    Task<ListResultDto<CategoryDto>> GetChildrenAsync(string definitionName, Guid? id);
    Task<PagedResultDto<CategoryDto>> GetPagedChildrenAsync(string definitionName, Guid? id,
        CategoryGetChildrenInput input);
    Task<CategoryDto> CreateAsync(string definitionName, CategoryCreateOrUpdateInput input);
    Task<CategoryDto> UpdateAsync(string definitionName, Guid id, CategoryCreateOrUpdateInput input);
    Task DeleteAsync(string definitionName, Guid id);
    Task DeleteManyAsync(string definitionName, IEnumerable<Guid> ids);
}