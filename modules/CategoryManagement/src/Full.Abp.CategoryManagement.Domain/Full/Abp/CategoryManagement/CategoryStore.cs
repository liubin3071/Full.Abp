using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Full.Abp.Categories;
using Full.Abp.Trees;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace Full.Abp.CategoryManagement;

public class CategoryStore : ICategoryStore
{
    private readonly ITreeEntityService<Category, Guid> _service;
    protected IObjectMapper ObjectMapper { get; }

    public CategoryStore(IAbpLazyServiceProvider lazyServiceProvider, string providerName, string providerKey)
    {
        var provider = lazyServiceProvider
            .LazyGetRequiredService<ITreeEntityServiceFactory<Category, CategoryRelation, Guid>>();
        _service = provider.Create(providerName, providerKey);
        ObjectMapper = lazyServiceProvider.LazyGetRequiredService<IObjectMapper>();
    }

    public async Task<CategoryInfo> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _service.GetAsync(id, false, cancellationToken);
        return ObjectMapper.Map<Category, CategoryInfo>(category);
    }

    public async Task<CategoryInfo?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _service.FindAsync(id, cancellationToken: cancellationToken);
        return category == null ? null : ObjectMapper.Map<Category, CategoryInfo>(category);
    }

    public async Task<List<CategoryInfo>> GetAncestorsAsync(string definitionName, Guid nodeId,
        CancellationToken cancellationToken = default)
    {
        var list = await _service.GetAncestorsAsync(definitionName, nodeId, cancellationToken: cancellationToken);
        return ObjectMapper.Map<List<Category>, List<CategoryInfo>>(list);
    }

    public async Task<List<CategoryInfo>> GetDescendantsAsync(string definitionName, Guid? nodeId,
        int? maxDistance = null,
        CancellationToken cancellationToken = default)
    {
        var list = await _service.GetDescendantsAsync(definitionName, nodeId, maxDistance,
            cancellationToken: cancellationToken);
        return ObjectMapper.Map<List<Category>, List<CategoryInfo>>(list);
    }

    public async Task<List<TreeNodeWrapper<CategoryInfo>>> GetTreeAsync(string definitionName, Guid? nodeId,
        CancellationToken cancellationToken = default)
    {
        var list = await _service.GetTreeAsync(definitionName, nodeId, cancellationToken: cancellationToken);
        return list.TreeSelect(c => new TreeNodeWrapper<CategoryInfo>() {
            Value = ObjectMapper.Map<Category, CategoryInfo>(c.Value),
        }).ToList();
    }

    public Task<bool> HasChildrenAsync(string definitionName, Guid? nodeId,
        CancellationToken cancellationToken = default)
    {
        return _service.HasChildrenAsync(definitionName, nodeId, cancellationToken);
    }

    public async Task<List<CategoryInfo>> GetChildrenAsync(string definitionName, Guid? nodeId,
        CancellationToken cancellationToken = default)
    {
        var list = await _service.GetChildrenAsync(definitionName, nodeId, cancellationToken: cancellationToken);
        return ObjectMapper.Map<List<Category>, List<CategoryInfo>>(list);
    }

    public async Task<List<CategoryInfo>> GetChildrenAsync(string definitionName, Guid? nodeId, int skipCount,
        int maxResultCount, string? sorting = null, CancellationToken cancellationToken = default)
    {
        var list = await _service.GetChildrenAsync(definitionName, nodeId, skipCount, maxResultCount, sorting: sorting,
            cancellationToken: cancellationToken);
        return ObjectMapper.Map<List<Category>, List<CategoryInfo>>(list);
    }

    public Task<int> GetChildrenCountAsync(string definitionName, Guid? nodeId,
        CancellationToken cancellationToken = default)
    {
        return _service.GetChildrenCountAsync(definitionName, nodeId, cancellationToken);
    }

    public async Task<CategoryInfo> GetRootAsync(string definitionName, Guid nodeId,
        CancellationToken cancellationToken = default)
    {
        var category = await _service.GetRootAsync(definitionName, nodeId, cancellationToken: cancellationToken);
        return ObjectMapper.Map<Category, CategoryInfo>(category);
    }

    public async Task<CategoryInfo?> GetParentAsync(string definitionName, Guid nodeId,
        CancellationToken cancellationToken = default)
    {
        var category = await _service.GetParentAsync(definitionName, nodeId, cancellationToken: cancellationToken);
        return category == null ? null : ObjectMapper.Map<Category, CategoryInfo>(category);
    }

    public async Task<CategoryInfo> CreateAsync(string definitionName, CategoryInfo entity, Guid? parentId = default,
        CancellationToken cancellationToken = default)
    {
        var category = await _service.CreateAsync(definitionName, ObjectMapper.Map<CategoryInfo, Category>(entity),
            parentId,
            cancellationToken: cancellationToken);
        return ObjectMapper.Map<Category, CategoryInfo>(category);
    }

    public Task EnsureParentAsync(string definitionName, Guid id, Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        return _service.EnsureParentAsync(definitionName, id, parentId, cancellationToken);
    }

    public Task DeleteAsync(string definitionName, Guid id, CancellationToken cancellationToken = default)
    {
        return _service.DeleteAsync(definitionName, id, cancellationToken);
    }

    public Task DeleteAllAsync(string definitionName, CancellationToken cancellationToken = default)
    {
        return _service.DeleteAllAsync(definitionName, cancellationToken);
    }
}