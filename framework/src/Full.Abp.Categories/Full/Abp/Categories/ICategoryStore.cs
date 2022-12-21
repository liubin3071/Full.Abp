using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Full.Abp.Categories;

public interface ICategoryStore
{
    Task<CategoryInfo> GetAsync(string providerName, string providerKey, string definitionName, Guid id,
        CancellationToken cancellationToken = default);

    Task<CategoryInfo?> FindAsync(string providerName, string providerKey, string definitionName, Guid id,
        CancellationToken cancellationToken = default);

    Task<CategoryInfo> GetByNameAsync(string providerName, string providerKey, string definitionName, string name,
        CancellationToken cancellationToken = default);

    Task<CategoryInfo?> FindByNameAsync(string providerName, string providerKey, string definitionName, string name,
        CancellationToken cancellationToken = default);

    Task<List<CategoryInfo>> GetAncestorsAsync(string definitionName, Guid nodeId,
        CancellationToken cancellationToken = default);

    Task<List<CategoryInfo>> GetDescendantsAsync(string definitionName, Guid? nodeId, int? maxDistance = null,
        CancellationToken cancellationToken = default);

    Task<List<TreeNodeWrapper<CategoryInfo>>> GetTreeAsync(string definitionName, Guid? nodeId,
        CancellationToken cancellationToken = default);

    Task<bool> HasChildrenAsync(string definitionName, Guid? nodeId, CancellationToken cancellationToken = default);

    Task<List<CategoryInfo>> GetChildrenAsync(string definitionName, Guid? nodeId,
        CancellationToken cancellationToken = default);

    Task<List<CategoryInfo>> GetChildrenAsync(string definitionName, Guid? nodeId, int skipCount,
        int maxResultCount, string? sorting = null,
        CancellationToken cancellationToken = default);

    Task<int> GetChildrenCountAsync(string definitionName, Guid? nodeId, CancellationToken cancellationToken = default);

    Task<CategoryInfo> GetRootAsync(string definitionName, Guid nodeId,
        CancellationToken cancellationToken = default);

    Task<CategoryInfo?> GetParentAsync(string definitionName, Guid nodeId,
        CancellationToken cancellationToken = default);

    Task<CategoryInfo> CreateAsync(string definitionName, CategoryInfo entity, Guid? parentId = default,
        CancellationToken cancellationToken = default);

    Task EnsureParentAsync(string definitionName, Guid id, Guid? parentId,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string definitionName, Guid id, CancellationToken cancellationToken = default);

    Task DeleteAllAsync(string definitionName, CancellationToken cancellationToken = default);
}