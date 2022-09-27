using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Full.Abp.Categories;
using Full.Abp.Trees;
using Volo.Abp.Application.Dtos;

namespace Full.Abp.CategoryManagement.Full.Abp.Categories;

public class CategoryAppService : CategoryManagementAppService, ICategoryAppService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryAppService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<Guid> GetTreeIdAsync(string definitionName)
    {
        return _categoryRepository.GetTreeIdAsync(definitionName, CurrentTenant.GetCategoryProviderName(), CurrentTenant.Id.ToString());
    }

    public async Task<ListResultDto<CategoryDto>> GetAncestorsAsync(Guid id)
    {
        var ancestors = await _categoryRepository.GetAncestorsAsync(id);
        return new ListResultDto<CategoryDto>(ObjectMapper.Map<IEnumerable<Category>, List<CategoryDto>>(ancestors));
    }

    public async Task<CategoryDto?> GetAncestorAsync(GetAncestorInput input)
    {
        var ancestor = await _categoryRepository.GetAncestorAsync(input.Id, input.Deepin);
        return ObjectMapper.Map<Category, CategoryDto>(ancestor);
    }

    public async Task<ListResultDto<CategoryDto>> GetAllAsync(string definitionName)
    {
        var all = await _categoryRepository.GetAllAsync(definitionName, CurrentTenant.GetCategoryProviderName(), CurrentTenant.Id.ToString());
        return new ListResultDto<CategoryDto>(ObjectMapper.Map<List<Category>, List<CategoryDto>>(all));
    }
    
    public async Task<List<CategoryDto>> GetTreeAsync(string definitionName)
    {
        var all = (await _categoryRepository.GetTreeAsync(definitionName, CurrentTenant.GetCategoryProviderName(),
                CurrentTenant.Id.ToString()))
            .OrderBy(c => c.Value.Sequence)
            .ToList();
        return all.TreeSelect(wrapper => new CategoryDto() { Id = wrapper.Value.Id, Name = wrapper.Value.Name, Sequence = wrapper.Value.Sequence }, wrapper => wrapper.Children.OrderBy(c=>c.Value.Sequence), (parent, children) => parent.Children = children ).ToList();
    }
    public async Task<ListResultDto<CategoryDto>> GetDescendantsAsync(CategoryGetDescendantsInput input)
    {
        var ds = await _categoryRepository.GetDescendantsAsync(input.Id, input.MaxDistance);
        return new ListResultDto<CategoryDto>(ObjectMapper.Map<List<Category>, List<CategoryDto>>(ds));
    }

    public Task<bool> HasChildrenAsync(Guid id)
    {
        return _categoryRepository.HasChildrenAsync(id);
    }

    public Task<int> GetChildrenCountAsync(Guid id)
    {
        return _categoryRepository.GetChildrenCountAsync(id);
    }

    public async Task<ListResultDto<CategoryDto>> GetChildrenAsync(Guid id)
    {
        var children = (await _categoryRepository.GetChildrenAsync(id))
            .OrderBy(category => category.Sequence)
            .ToList();
        return new ListResultDto<CategoryDto>(ObjectMapper.Map<List<Category>, List<CategoryDto>>(children));
    }

    public async Task<PagedResultDto<CategoryDto>> GetPagedChildrenAsync(CategoryGetChildrenInput input)
    {
        var totalCount = await _categoryRepository.GetChildrenCountAsync(input.Id);
        var children =
            await _categoryRepository.GetPagedChildrenAsync(input.Id, input.SkipCount, input.MaxResultCount,
                input.Sorting);
        return new PagedResultDto<CategoryDto>(totalCount,
            ObjectMapper.Map<List<Category>, List<CategoryDto>>(children));
    }

    public async Task<CategoryDto> CreateAsync(CategoryCreateInput input)
    {
        var category = ObjectMapper.Map<CategoryCreateInput, Category>(input);
        await _categoryRepository.InsertAsync(category, input.ParentId);
        return ObjectMapper.Map<Category, CategoryDto>(category);
    }

    public async Task<CategoryDto> UpdateAsync(Guid id, CategoryUpdateInput input)
    {
        var category = await _categoryRepository.GetAsync(id);
        ObjectMapper.Map(input, category);
        if (input.ParentId != Guid.Empty)
        {
            await _categoryRepository.EnsureParentAsync(id, input.ParentId);
        }
        return ObjectMapper.Map<Category, CategoryDto>(category);
    }

    public Task DeleteAsync(Guid id)
    {
        return _categoryRepository.DeleteAsync(id);
    }

    public async Task DeleteManyAsync(IEnumerable<Guid> ids)
    {
        await _categoryRepository.DeleteManyAsync(ids);
    }

    public async Task<CategoryDto> GetAsync(Guid id)
    {
        var category = await _categoryRepository.GetAsync(id);
        return ObjectMapper.Map<Category, CategoryDto>(category);
    }
}