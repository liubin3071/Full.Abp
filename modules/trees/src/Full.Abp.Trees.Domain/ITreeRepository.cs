using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Full.Abp.Trees;

public interface ITreeRepository<TTreeEntity, TKey> : IBasicRepository<TTreeEntity, TKey>
    where TTreeEntity : class, IEntity<TKey>
{
    Task EnsureCreateTree(string providerType, string providerName, string providerKey);

    Task<TKey> GetTreeIdAsync(string providerType, string providerName, string? providerKey,
        CancellationToken cancellationToken = default);

    Task<List<TreeNodeWrapper<TTreeEntity>>> GetTreeAsync(string providerType, string providerName,
        string? providerKey,
        bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<TTreeEntity>> GetAllAsync(string providerType, string providerName,
        string? providerKey,
        bool includeDetails = true, CancellationToken cancellationToken = default);

    Task<List<TTreeEntity>> GetListByIdsAsync(IEnumerable<TKey> ids, bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<List<TTreeEntity>> GetAncestorsAsync(TKey id, bool includeSelf = false, bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<TTreeEntity> GetAncestorAsync(TKey id, int distance, bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<int> GetDeepinAsync(TKey id, CancellationToken cancellationToken = default);

    Task<List<TTreeEntity>> GetDescendantsAsync(TKey id, int? maxDistance = null,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<bool> HasChildrenAsync(TKey id, CancellationToken cancellationToken = default);
    Task<int> GetChildrenCountAsync(TKey id, CancellationToken cancellationToken = default);

    Task<List<TTreeEntity>> GetChildrenAsync(TKey id, bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<List<TTreeEntity>> GetPagedChildrenAsync(TKey id, int skipCount, int maxResultCount,
        string sorting, bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<TTreeEntity> InsertAsync(TTreeEntity entity, TKey parentId, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task InsertManyAsync(IEnumerable<TTreeEntity> entities, TKey parentId, bool autoSave = false,
        CancellationToken cancellationToken = default);


    Task EnsureParentAsync(TKey id, TKey parentId, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteWithoutDescendantsAsync(TKey id, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteWithoutDescendantsAsync(TTreeEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteWithoutDescendantsAsync(Expression<Func<TTreeEntity, bool>> predicate,
        bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteManyWithoutDescendantAsync(IEnumerable<TTreeEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task DeleteManyWithoutDescendantAsync(IEnumerable<TKey> ids, bool autoSave = false,
        CancellationToken cancellationToken = default);
}