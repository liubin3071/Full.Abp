using System.Linq.Expressions;

namespace Full.Abp.Categories;

public interface ICategoryService
{
    Task EnsureCreateTree(string providerType, string providerName, string providerKey);

    Task<Guid> GetTreeIdAsync(string providerType, string providerName, string? providerKey,
        CancellationToken cancellationToken = default);

    Task<List<TreeNodeWrapper<CategoryInfo>>> GetTreeAsync(string providerType, string providerName,
        string? providerKey,
        bool includeDetails = true, CancellationToken cancellationToken = default);
    
    Task<List<CategoryInfo>> GetAllAsync(string providerType, string providerName,
        string? providerKey,
        bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<CategoryInfo>> GetListByIdsAsync(IEnumerable<Guid> ids);

    Task<List<CategoryInfo>> GetAncestorsAsync(Guid id, bool includeSelf = false, bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<CategoryInfo> GetAncestorAsync(Guid id, int distance, bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<int> GetDeepinAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<CategoryInfo>> GetDescendantsAsync(Guid id, int? maxDistance = null,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<bool> HasChildrenAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> GetChildrenCountAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<CategoryInfo>> GetChildrenAsync(Guid id, bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<List<CategoryInfo>> GetPagedChildrenAsync(Guid id, int skipCount, int maxResultCount,
        string sorting, bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<CategoryInfo> InsertAsync(CategoryInfo entity, Guid parentId, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task InsertManyAsync(IEnumerable<CategoryInfo> entities, Guid parentId, bool autoSave = false,
        CancellationToken cancellationToken = default);


    Task EnsureParentAsync(Guid id, Guid parentId, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteWithoutDescendantsAsync(Guid id, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteWithoutDescendantsAsync(CategoryInfo entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteManyWithoutDescendantAsync(IEnumerable<CategoryInfo> entities, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteManyWithoutDescendantAsync(IEnumerable<Guid> ids, bool autoSave = false,
        CancellationToken cancellationToken = default);
}