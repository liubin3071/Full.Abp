using System;
using Volo.Abp.Domain.Entities;

namespace Full.Abp.Trees;

public interface ITreeEntityServiceFactory<TEntity, TRelation, TKey>
    where TEntity : class, IEntity<TKey>
    where TRelation : TreeRelation<TKey>
    where TKey : struct
{
    ITreeEntityService<TEntity, TKey> Create(string providerName, string providerKey);
    ITreeEntityService<TEntity, TKey> CreateForTenantOrGlobal(Guid? tenantId);
    ITreeEntityService<TEntity, TKey> CreateForGlobal();
    ITreeEntityService<TEntity, TKey> CreateForUser(Guid userId);
}