using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Full.Abp.Trees;

public class TreeEntityService<TEntity, TRelation, TKey> : ITreeEntityService<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TRelation : TreeRelation<TKey>
    where TKey : struct
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; }
    public string ProviderName { get; }
    public string ProviderKey { get; }
    protected IRepository<TEntity, TKey> Repository { get; }
    public ITreeRelationRepository<TRelation, TKey> RelationRepository { get; }

    internal TreeEntityService(IAbpLazyServiceProvider lazyServiceProvider, string providerName,
        string providerKey)
    {
        LazyServiceProvider = lazyServiceProvider;
        Repository = LazyServiceProvider.LazyGetRequiredService<IRepository<TEntity, TKey>>();
        RelationRepository = lazyServiceProvider.LazyGetRequiredService<ITreeRelationRepository<TRelation, TKey>>();

        ProviderName = providerName;
        ProviderKey = providerKey;
    }

    public Task<TEntity> GetAsync(TKey id, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return Repository.GetAsync(id, includeDetails, cancellationToken);
    }

    public Task<TEntity?> FindAsync(TKey id, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return Repository.FindAsync(id, includeDetails, cancellationToken)!;
    }

    public Task<IQueryable<TEntity>> GetQueryableAsync(bool includeDetails = false)
    {
        return includeDetails ? Repository.WithDetailsAsync() : Repository.GetQueryableAsync();
    }

    public async Task<List<TEntity>> GetAncestorsAsync(string providerType, TKey nodeId, bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var aRelations = (await RelationRepository.GetAncestorsAsync(providerType, ProviderName, ProviderKey, nodeId,
                cancellationToken))
            .OrderByDescending(c => c.Distance);

        return (from treeRelation in aRelations
            join entity in (await GetQueryableAsync(includeDetails)) on treeRelation.Ancestor equals entity.Id
            orderby treeRelation.Distance descending
            select entity).ToList();
    }

    public async Task<List<TEntity>> GetDescendantsAsync(string providerType, TKey? nodeId, int? maxDistance = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var dIds = await RelationRepository.GetDescendantsAsync(providerType, ProviderName, ProviderKey, nodeId,
            maxDistance, cancellationToken: cancellationToken);
        var queryable = (await GetQueryableAsync()).Where(c => dIds.Contains(c.Id));
        return queryable.ToList();
    }

    public virtual async Task<List<TreeNodeWrapper<TEntity>>> GetTreeAsync(string providerType, TKey? nodeId, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var dIds = await RelationRepository.GetDescendantsAsync(providerType, ProviderName, ProviderKey, nodeId,
            null, cancellationToken: cancellationToken);
        var parentRelations =
            (await RelationRepository.GetQueryableAsync(providerType, ProviderName, ProviderKey)).Where(c =>
                c.Distance == 1);
        var entities = (await GetQueryableAsync());
        var list = (from id in dIds
                join entity in entities on id equals entity.Id
                join relation in parentRelations on id equals relation.Descendant into relations
                from relation in relations.DefaultIfEmpty()
                select new { Value = entity, ParentId = relation == null ? (TKey?)null : relation.Ancestor })
            .ToList();

        return list.ToTreeList(
                parentKeySelector: src => src.ParentId,
                dst => dst.Value.Id,
                src => new TreeNodeWrapper<TEntity>() { Value = src.Value }, nodeId)
            ;
    }

    public async Task<bool> HasChildrenAsync(string providerType, TKey? nodeId, CancellationToken cancellationToken = default)
    {
        return (await RelationRepository.GetDescendantsAsync(providerType, ProviderName, ProviderKey, nodeId,
            1, cancellationToken: cancellationToken)).Any();
    }

    public Task<List<TEntity>> GetChildrenAsync(string providerType, TKey? nodeId, bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        return GetDescendantsAsync(providerType,nodeId, 1, includeDetails, cancellationToken);
    }

    public async Task<List<TEntity>> GetChildrenAsync(string providerType, TKey? nodeId, int skipCount,
        int maxResultCount,
        Expression<Func<TEntity, bool>>? predicate = null, string? sorting = null,
        bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var childIds = await RelationRepository.GetDescendantsAsync(providerType, ProviderName, ProviderKey, nodeId, 1,
            false, cancellationToken);
        return (await GetQueryableAsync(includeDetails))
            .Where(c => childIds.Contains(c.Id))
            .WhereIf(predicate != null, predicate)
            .OrderBy(sorting.IsNullOrEmpty() ? "Id desc" : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToList();
    }

    public async Task<int> GetChildrenCountAsync(string providerType, TKey? nodeId, CancellationToken cancellationToken = default)
    {
        return (await RelationRepository.GetDescendantsAsync(providerType, ProviderName, ProviderKey, nodeId, 1,
                cancellationToken: cancellationToken))
            .Count();
    }

    public async Task<TEntity> GetRootAsync(string providerType, TKey nodeId, bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var rootId = (await RelationRepository.GetAncestorsAsync(providerType, ProviderName, ProviderKey, nodeId,
                cancellationToken))
            .OrderByDescending(c => c.Distance)
            .Select(c => c.Ancestor)
            .First();

        return await Repository.FindAsync(rootId, includeDetails, cancellationToken);
    }

    public async Task<TEntity?> GetParentAsync(string providerType, TKey nodeId, bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var parentId = (await RelationRepository.GetAncestorsAsync(providerType, ProviderName, ProviderKey, nodeId,
                cancellationToken))
            .Where(c => c.Distance == 1)
            .Select(c => (TKey?)c.Ancestor)
            .FirstOrDefault();

        if (parentId == null)
        {
            return null;
        }

        return await Repository.FindAsync(parentId.Value, includeDetails, cancellationToken);
    }

    [UnitOfWork]
    public async Task<TEntity> CreateAsync(string providerType, TEntity entity, TKey? parentId = default,
        CancellationToken cancellationToken = default)
    {
        await Repository.InsertAsync(entity, true, cancellationToken);
        await RelationRepository.EnsureParentAsync(providerType, ProviderName, ProviderKey, entity.Id, parentId,
            false, cancellationToken);
        return entity;
    }

    [UnitOfWork]
    public Task EnsureParentAsync(string providerType, TKey id, TKey? parentId, CancellationToken cancellationToken = default)
    {
        return RelationRepository.EnsureParentAsync(providerType, ProviderName, ProviderKey, id, parentId,
            false, cancellationToken);
    }

    [UnitOfWork]
    public async Task DeleteAsync(string providerType, TKey id, CancellationToken cancellationToken = default)
    {
        var ids = await RelationRepository.DeleteAsync(providerType, ProviderName, ProviderKey, id, true,
            false, cancellationToken);
        await Repository.DeleteAsync(c => ids.Contains(c.Id), false, cancellationToken);
    }

    [UnitOfWork]
    public async Task DeleteAllAsync(string providerType, CancellationToken cancellationToken = default)
    {
        var ids = await RelationRepository.DeleteAsync(providerType, ProviderName, ProviderKey, null, true,
            false, cancellationToken);
        await Repository.DeleteAsync(c => ids.Contains(c.Id), false, cancellationToken);
    }
}