using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Full.Abp.Trees;

public interface ITreeEntityService<TEntity, TKey> where TKey : struct
{
    Task<TEntity> GetAsync(TKey id, bool includeDetails = false, CancellationToken cancellationToken = default);
    Task<TEntity?> FindAsync(TKey id, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<IQueryable<TEntity>> GetQueryableAsync(bool includeDetails = false);

    Task<List<TEntity>> GetAncestorsAsync(string providerType, TKey nodeId, bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetDescendantsAsync(string providerType, TKey? nodeId, int? maxDistance = null, bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<List<TreeNodeWrapper<TEntity>>> GetTreeAsync(string providerType, TKey? nodeId, bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<bool> HasChildrenAsync(string providerType, TKey? nodeId, CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetChildrenAsync(string providerType, TKey? nodeId, bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetChildrenAsync(string providerType, TKey? nodeId, int skipCount, int maxResultCount,
        Expression<Func<TEntity, bool>>? predicate = null, string? sorting = null,
        bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<int> GetChildrenCountAsync(string providerType, TKey? nodeId, CancellationToken cancellationToken = default);

    Task<TEntity> GetRootAsync(string providerType, TKey nodeId, bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetParentAsync(string providerType, TKey nodeId, bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<TEntity> CreateAsync(string providerType, TEntity entity, TKey? parentId = default, CancellationToken cancellationToken = default);

    Task EnsureParentAsync(string providerType, TKey id, TKey? parentId, CancellationToken cancellationToken = default);

    Task DeleteAsync(string providerType, TKey id, CancellationToken cancellationToken = default);

    Task DeleteAllAsync(string providerType, CancellationToken cancellationToken = default);
}