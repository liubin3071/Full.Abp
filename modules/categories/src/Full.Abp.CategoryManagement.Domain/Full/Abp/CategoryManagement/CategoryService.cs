using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Full.Abp.Categories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace Full.Abp.CategoryManagement;

public class CategoryService : ICategoryService, ITransientDependency
{
    private readonly IObjectMapper _objectMapper;
    private readonly ICategoryRepository _categoryRepositoryImplementation;

    public CategoryService(IObjectMapper objectMapper, ICategoryRepository categoryRepositoryImplementation)
    {
        _objectMapper = objectMapper;
        _categoryRepositoryImplementation = categoryRepositoryImplementation;
    }

    public Task<List<Category>> GetListAsync(bool includeDetails = false,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _categoryRepositoryImplementation.GetListAsync(includeDetails, cancellationToken);
    }

    public Task<long> GetCountAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return _categoryRepositoryImplementation.GetCountAsync(cancellationToken);
    }

    public async Task<List<CategoryInfo>> GetPagedListAsync(int skipCount, int maxResultCount, string sorting,
        bool includeDetails = false,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var list = await _categoryRepositoryImplementation.GetPagedListAsync(skipCount, maxResultCount, sorting,
            includeDetails, cancellationToken);
        return _objectMapper.Map<List<Category>, List<CategoryInfo>>(list);
    }

    public async Task<CategoryInfo> InsertAsync(CategoryInfo entity, bool autoSave = false,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var category =
            await _categoryRepositoryImplementation.InsertAsync(_objectMapper.Map<CategoryInfo, Category>(entity),
                autoSave, cancellationToken);
        return _objectMapper.Map<Category, CategoryInfo>(category);
    }

    public Task InsertManyAsync(IEnumerable<CategoryInfo> entities, bool autoSave = false,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _categoryRepositoryImplementation.InsertManyAsync(
            _objectMapper.Map<IEnumerable<CategoryInfo>, List<Category>>(entities), autoSave, cancellationToken);
    }

    public async Task<CategoryInfo> UpdateAsync(CategoryInfo entity, bool autoSave = false,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var category =
            await _categoryRepositoryImplementation.UpdateAsync(_objectMapper.Map<CategoryInfo, Category>(entity),
                autoSave, cancellationToken);
        return _objectMapper.Map<Category, CategoryInfo>(category);
    }

    public Task UpdateManyAsync(IEnumerable<CategoryInfo> entities, bool autoSave = false,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _categoryRepositoryImplementation.UpdateManyAsync(
            _objectMapper.Map<IEnumerable<CategoryInfo>, List<Category>>(entities), autoSave, cancellationToken);
    }

    public Task DeleteAsync(CategoryInfo entity, bool autoSave = false,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _categoryRepositoryImplementation.DeleteAsync(_objectMapper.Map<CategoryInfo, Category>(entity),
            autoSave, cancellationToken);
    }

    public Task DeleteManyAsync(IEnumerable<CategoryInfo> entities, bool autoSave = false,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _categoryRepositoryImplementation.DeleteManyAsync(
            _objectMapper.Map<IEnumerable<CategoryInfo>, List<Category>>(entities), autoSave, cancellationToken);
    }

    public async Task<CategoryInfo> GetAsync(Guid id, bool includeDetails = true,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var category = await _categoryRepositoryImplementation.GetAsync(id, includeDetails, cancellationToken);
        return _objectMapper.Map<Category, CategoryInfo>(category);
    }

    public async Task<CategoryInfo> FindAsync(Guid id, bool includeDetails = true,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var category = await _categoryRepositoryImplementation.FindAsync(id, includeDetails, cancellationToken);
        return _objectMapper.Map<Category, CategoryInfo>(category);
    }

    public Task DeleteAsync(Guid id, bool autoSave = false,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _categoryRepositoryImplementation.DeleteAsync(id, autoSave, cancellationToken);
    }

    public Task DeleteManyAsync(IEnumerable<Guid> ids, bool autoSave = false,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return _categoryRepositoryImplementation.DeleteManyAsync(ids, autoSave, cancellationToken);
    }

    public Task EnsureCreateTree(string providerType, string providerName, string providerKey)
    {
        return _categoryRepositoryImplementation.EnsureCreateTree(providerType, providerName, providerKey);
    }

    public Task<Guid> GetTreeIdAsync(string providerType, string providerName, string? providerKey,
        CancellationToken cancellationToken = default)
    {
        return _categoryRepositoryImplementation.GetTreeIdAsync(providerType, providerName, providerKey,
            cancellationToken);
    }

    public async Task<List<TreeNodeWrapper<CategoryInfo>>> GetTreeAsync(string providerType, string providerName,
        string? providerKey, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var list = await _categoryRepositoryImplementation.GetTreeAsync(providerType, providerName, providerKey,
            includeDetails,
            cancellationToken);
        return _objectMapper.Map<List<TreeNodeWrapper<Category>>, List<TreeNodeWrapper<CategoryInfo>>>(list);
    }

    public async Task<List<CategoryInfo>> GetAllAsync(string providerType, string providerName, string? providerKey,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var list = await _categoryRepositoryImplementation.GetAllAsync(providerType, providerName, providerKey,
            includeDetails,
            cancellationToken);
        return _objectMapper.Map<List<Category>, List<CategoryInfo>>(list);
    }

    public async Task<List<CategoryInfo>> GetListByIdsAsync(IEnumerable<Guid> ids)
    {
        var list = await _categoryRepositoryImplementation.GetListByIdsAsync(ids, false);
        return _objectMapper.Map<List<Category>, List<CategoryInfo>>(list);
    }

    public async Task<List<CategoryInfo>> GetAncestorsAsync(Guid id, bool includeSelf = false, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var list = await _categoryRepositoryImplementation.GetAncestorsAsync(id, includeSelf, includeDetails, cancellationToken);
        return _objectMapper.Map<List<Category>, List<CategoryInfo>>(list);
    }

    public async Task<CategoryInfo> GetAncestorAsync(Guid id, int distance, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var category =
            await _categoryRepositoryImplementation.GetAncestorAsync(id, distance, includeDetails, cancellationToken);
        return _objectMapper.Map<Category, CategoryInfo>(category);
    }

    public Task<int> GetDeepinAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _categoryRepositoryImplementation.GetDeepinAsync(id, cancellationToken);
    }

    public async Task<List<CategoryInfo>> GetDescendantsAsync(Guid id, int? maxDistance = null,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var list = await _categoryRepositoryImplementation.GetDescendantsAsync(id, maxDistance, includeDetails,
            cancellationToken);
        return _objectMapper.Map<List<Category>, List<CategoryInfo>>(list);
    }

    public Task<bool> HasChildrenAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _categoryRepositoryImplementation.HasChildrenAsync(id, cancellationToken);
    }

    public Task<int> GetChildrenCountAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _categoryRepositoryImplementation.GetChildrenCountAsync(id, cancellationToken);
    }

    public async Task<List<CategoryInfo>> GetChildrenAsync(Guid id, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var list = await _categoryRepositoryImplementation.GetChildrenAsync(id, includeDetails, cancellationToken);
        return _objectMapper.Map<List<Category>, List<CategoryInfo>>(list);
    }

    public async Task<List<CategoryInfo>> GetPagedChildrenAsync(Guid id, int skipCount, int maxResultCount,
        string sorting,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var list = await _categoryRepositoryImplementation.GetPagedChildrenAsync(id, skipCount, maxResultCount, sorting,
            includeDetails, cancellationToken);
        return _objectMapper.Map<List<Category>, List<CategoryInfo>>(list);
    }

    public async Task<CategoryInfo> InsertAsync(CategoryInfo entity, Guid parentId, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepositoryImplementation.InsertAsync(
            _objectMapper.Map<CategoryInfo, Category>(entity), parentId, autoSave, cancellationToken);
        return _objectMapper.Map<Category, CategoryInfo>(category);
    }

    public Task InsertManyAsync(IEnumerable<CategoryInfo> entities, Guid parentId, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        return _categoryRepositoryImplementation.InsertManyAsync(
            _objectMapper.Map<IEnumerable<CategoryInfo>, List<Category>>(entities), parentId, autoSave,
            cancellationToken);
    }

    public Task EnsureParentAsync(Guid id, Guid parentId, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        return _categoryRepositoryImplementation.EnsureParentAsync(id, parentId, autoSave, cancellationToken);
    }

    public Task DeleteWithoutDescendantsAsync(Guid id, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        return _categoryRepositoryImplementation.DeleteWithoutDescendantsAsync(id, autoSave, cancellationToken);
    }

    public Task DeleteWithoutDescendantsAsync(CategoryInfo entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        return _categoryRepositoryImplementation.DeleteWithoutDescendantsAsync(
            _objectMapper.Map<CategoryInfo, Category>(entity), autoSave, cancellationToken);
    }

    public Task DeleteManyWithoutDescendantAsync(IEnumerable<CategoryInfo> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        return _categoryRepositoryImplementation.DeleteManyWithoutDescendantAsync(
            _objectMapper.Map<IEnumerable<CategoryInfo>, List<Category>>(entities), autoSave,
            cancellationToken);
    }

    public Task DeleteManyWithoutDescendantAsync(IEnumerable<Guid> ids, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        return _categoryRepositoryImplementation.DeleteManyWithoutDescendantAsync(ids, autoSave, cancellationToken);
    }
}