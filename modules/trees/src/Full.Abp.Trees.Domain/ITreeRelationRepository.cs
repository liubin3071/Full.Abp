using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Full.Abp.Trees;

public interface ITreeRelationRepository<TRelation, TKey> : IRepository
    where TKey : struct where TRelation : class, IEntity
{
    Task<IQueryable<TRelation>> GetQueryableAsync(string providerType, string providerName,
        string providerKey);

    Task<IQueryable<TKey>> GetDescendantsAsync(string providerType, string providerName, string providerKey,
        TKey? nodeId, int? maxDistance = null,
        bool includeSelf = false,
        CancellationToken cancellationToken = default);

    Task<IQueryable<TRelation>> GetAncestorsAsync(string providerType, string providerName, string providerKey,
        TKey nodeId, CancellationToken cancellationToken = default);

    Task EnsureParentAsync(string providerType, string providerName, string providerKey,
        TKey nodeId, TKey? parentId, bool autoSave = false, CancellationToken cancellationToken = default);

    Task<TKey[]> DeleteAsync(string providerType, string providerName, string providerKey,
        TKey? nodeIdOrAll, bool includeDescendants = true, bool autoSave = false,
        CancellationToken cancellationToken = default);
}