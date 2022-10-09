using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Full.Abp.Categories;
using Full.Abp.CategoryManagement.Permissions;
using Full.Abp.Trees;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;

namespace Full.Abp.CategoryManagement.Full.Abp.CategoryManagement;

public class CategoryAppService : CategoryManagementAppService, ICategoryAppService
{
    private readonly ITreeEntityServiceFactory<Category, CategoryRelation, Guid> _categoryServiceFactory;

    private ITreeEntityService<Category, Guid> CategoryService =>
        _categoryServiceFactory.CreateForTenantOrGlobal(CurrentTenant.Id);


    public CategoryAppService(ITreeEntityServiceFactory<Category, CategoryRelation, Guid> categoryServiceFactory)
    {
        _categoryServiceFactory = categoryServiceFactory;
        // _categoryService = categoryServiceFactory.CreateForTenantOrGlobal(CurrentTenant.Id);
    }

    public async Task<CategoryDto> GetAsync(string definitionName, Guid id)
    {
        await this.AuthorizationService.CheckAsync(CategoryManagementPermissions.Get(definitionName).Default);
        var category = await CategoryService.GetAsync(id);
        var dto = ObjectMapper.Map<Category, CategoryDto>(category);
        var parent = await CategoryService.GetParentAsync(definitionName, id);
        if (parent != null)
        {
            dto.Parent = ObjectMapper.Map<Category, CategoryDto>(parent);
        }

        return dto;
    }

    public async Task<ListResultDto<CategoryDto>> GetAncestorsAsync(string definitionName, Guid id)
    {
        await this.AuthorizationService.CheckAsync(CategoryManagementPermissions.Get(definitionName).Default);
        var ancestors = await CategoryService.GetAncestorsAsync(definitionName, id);
        return new ListResultDto<CategoryDto>(ObjectMapper.Map<IEnumerable<Category>, List<CategoryDto>>(ancestors));
    }

    public async Task<CategoryDto?> GetParentAsync(string definitionName, Guid id)
    {
        await this.AuthorizationService.CheckAsync(CategoryManagementPermissions.Get(definitionName).Default);
        var parent = await CategoryService.GetParentAsync(definitionName, id);
        return parent == null ? null : ObjectMapper.Map<Category, CategoryDto>(parent);
    }

    public async Task<ListResultDto<CategoryDto>> GetDescendantsAsync(string definitionName, Guid? id,
        int? maxDistance = null)
    {
        await this.AuthorizationService.CheckAsync(CategoryManagementPermissions.Get(definitionName).Default);
        var descendants = await CategoryService.GetDescendantsAsync(definitionName, id, maxDistance);
        return new ListResultDto<CategoryDto>(ObjectMapper.Map<IEnumerable<Category>, List<CategoryDto>>(descendants));
    }

    public async Task<IEnumerable<CategoryDto>> GetTreeAsync(string definitionName)
    {
        await this.AuthorizationService.CheckAsync(CategoryManagementPermissions.Get(definitionName).Default);
        var tree = await CategoryService.GetTreeAsync(definitionName, null);
        return tree.TreeSelect(
                wrapper => new CategoryDto() {
                    Id = wrapper.Value.Id, Name = wrapper.Value.Name, Sequence = wrapper.Value.Sequence
                },
                wrapper => wrapper.Children.OrderBy(c => c.Value.Sequence)
            )
            .TreeOrderBy(c => c.Sequence)
            .ToList();
    }

    public async Task<bool> HasChildrenAsync(string definitionName, Guid? id)
    {
        await this.AuthorizationService.CheckAsync(CategoryManagementPermissions.Get(definitionName).Default);
        return await CategoryService.HasChildrenAsync(definitionName, id);
    }

    public async Task<int> GetChildrenCountAsync(string definitionName, Guid? id)
    {
        await this.AuthorizationService.CheckAsync(CategoryManagementPermissions.Get(definitionName).Default);
        return await CategoryService.GetChildrenCountAsync(definitionName, id);
    }

    public async Task<ListResultDto<CategoryDto>> GetChildrenAsync(string definitionName, Guid? id)
    {
        await this.AuthorizationService.CheckAsync(CategoryManagementPermissions.Get(definitionName).Default);
        var children = (await CategoryService.GetChildrenAsync(definitionName, id)).OrderBy(c => c.Sequence);
        return new ListResultDto<CategoryDto>(ObjectMapper.Map<IEnumerable<Category>, List<CategoryDto>>(children));
    }

    public async Task<PagedResultDto<CategoryDto>> GetPagedChildrenAsync(string definitionName, Guid? id,
        CategoryGetChildrenInput input)
    {
        await this.AuthorizationService.CheckAsync(CategoryManagementPermissions.Get(definitionName).Default);
        var query = (await CategoryService.GetQueryableAsync())
            .WhereIf(!string.IsNullOrEmpty(input.Filter), category => category.Name.Contains(input.Filter!));

        var totalCount = await CategoryService.GetChildrenCountAsync(definitionName, id);
        var list = await CategoryService.GetChildrenAsync(definitionName, id, input.SkipCount, input.MaxResultCount,
            c => string.IsNullOrEmpty(input.Filter) || c.Name.Contains(input.Filter), input.Sorting);
        return new PagedResultDto<CategoryDto>(totalCount,
            ObjectMapper.Map<IEnumerable<Category>, List<CategoryDto>>(list));
    }

    public async Task<CategoryDto> CreateAsync(string definitionName, CategoryCreateOrUpdateInput input)
    {
        await this.AuthorizationService.CheckAsync(CategoryManagementPermissions.Get(definitionName).Create);
        var category = ObjectMapper.Map<CategoryCreateOrUpdateInput, Category>(input);
        category = await CategoryService.CreateAsync(definitionName, category, input.ParentId);
        return ObjectMapper.Map<Category, CategoryDto>(category);
    }

    public async Task<CategoryDto> UpdateAsync(string definitionName, Guid id, CategoryCreateOrUpdateInput input)
    {
        await this.AuthorizationService.CheckAsync(CategoryManagementPermissions.Get(definitionName).Update);
        var category = await CategoryService.GetAsync(id);
        ObjectMapper.Map<CategoryCreateOrUpdateInput, Category>(input, category);
        await CategoryService.EnsureParentAsync(definitionName, id, input.ParentId);
        return ObjectMapper.Map<Category, CategoryDto>(category);
    }

    public async Task DeleteAsync(string definitionName, Guid id)
    {
        await this.AuthorizationService.CheckAsync(CategoryManagementPermissions.Get(definitionName).Delete);
        await CategoryService.DeleteAsync(definitionName, id);
    }

    public async Task DeleteManyAsync(string definitionName, IEnumerable<Guid> ids)
    {
        await this.AuthorizationService.CheckAsync(CategoryManagementPermissions.Get(definitionName).Delete);
        foreach (var id in ids)
        {
            await CategoryService.DeleteAsync(definitionName, id);
        }
    }
}