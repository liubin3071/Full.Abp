using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Full.Abp.Trees;

public class TreeEntityServiceFactory<TEntity, TRelation, TKey> : ITreeEntityServiceFactory<TEntity, TRelation, TKey>,
    ITransientDependency
    where TEntity : class, IEntity<TKey>
    where TRelation : TreeRelation<TKey>
    where TKey : struct
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

    public TreeEntityServiceFactory(AbpLazyServiceProvider lazyServiceProvider)
    {
        LazyServiceProvider = lazyServiceProvider;
    }

    public ITreeEntityService<TEntity, TKey> Create(string providerName, string providerKey)
    {
        return new TreeEntityService<TEntity, TRelation, TKey>(LazyServiceProvider, providerName, providerKey);
    }

    public ITreeEntityService<TEntity, TKey> CreateForTenantOrGlobal(Guid? tenantId)
    {
        return Create(tenantId.HasValue ? "T" : "G", tenantId.ToString());
    }

    public ITreeEntityService<TEntity, TKey> CreateForGlobal()
    {
        return Create("G", string.Empty);
    }

    public ITreeEntityService<TEntity, TKey> CreateForUser(Guid userId)
    {
        return Create("U", userId.ToString());
    }
}