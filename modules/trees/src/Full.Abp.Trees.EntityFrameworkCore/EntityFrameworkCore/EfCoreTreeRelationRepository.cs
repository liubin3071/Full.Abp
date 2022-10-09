using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Full.Abp.Trees.EntityFrameworkCore;

public class EfCoreTreeRelationRepository<TDbContext, TRelation, TKey>
    : EfCoreRepository<TDbContext, TRelation>, ITreeRelationRepository<TRelation, TKey>
    where TRelation : TreeRelation<TKey>, new()
    where TDbContext : IEfCoreDbContext
    where TKey : struct
{
    public EfCoreTreeRelationRepository(IDbContextProvider<TDbContext> dbContextProvider,
        IAbpLazyServiceProvider lazyServiceProvider) : base(
        dbContextProvider)
    {
        this.LazyServiceProvider = lazyServiceProvider;
    }

    public async Task<IQueryable<TRelation>> GetQueryableAsync(string providerType, string providerName,
        string providerKey)
    {
        return (await GetQueryableAsync())
            .Where(c => c.ProviderType == providerType && c.ProviderName == providerName &&
                        c.ProviderKey == providerKey);
    }

    public async Task<IQueryable<TKey>> GetDescendantsAsync(string providerType, string providerName,
        string providerKey, TKey? nodeId, int? maxDistance = null, bool includeSelf = false,
        CancellationToken cancellationToken = default)
    {
        var queryable = (await GetQueryableAsync(providerType, providerName, providerKey));

        if (nodeId.HasValue)
        {
            return queryable
                .Where(c => c.Ancestor.Equals(nodeId))
                .WhereIf(!includeSelf, c => c.Distance > 0)
                .WhereIf(maxDistance.HasValue, c => c.Distance <= maxDistance)
                .Select(c => c.Descendant);
        }

        var rootIds = queryable.Where(c => c.IsRoot).Select(c => c.Ancestor);
        return queryable
            .Where(c => rootIds.Contains(c.Ancestor))
            .WhereIf(maxDistance.HasValue, c => c.Distance <= maxDistance - 1)
            .Select(c => c.Descendant);
    }

    public async Task<IQueryable<TRelation>> GetAncestorsAsync(string providerType, string providerName,
        string providerKey,
        TKey nodeId, CancellationToken cancellationToken = default)
    {
        return (await GetQueryableAsync(providerType, providerName, providerKey))
            .Where(c => c.Descendant.Equals(nodeId) && c.Distance > 0)
            ;
    }

    public async Task EnsureParentAsync(string providerType, string providerName, string providerKey, TKey nodeId,
        TKey? parentId, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        if (!parentId.HasValue)
        {
            await CreateRootNodeAsync(providerType, providerName, providerKey, nodeId, autoSave, cancellationToken);
            return;
        }

        var queryable = await GetQueryableAsync(providerType, providerName, providerKey);

        // 检查父子关系是否已存在
        if (await queryable.AnyAsync(
                c => c.Ancestor.Equals(parentId) && c.Descendant.Equals(nodeId) && c.Distance == 1,
                cancellationToken: cancellationToken))
        {
            return;
        }

        // 检查新父节点是否存在
        if (!await queryable.AnyAsync(
                c => c.Ancestor.Equals(parentId) && c.Descendant.Equals(parentId) && c.Distance == 0,
                cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException("Parent Notfound.");
        }

        if (await queryable.AnyAsync(c => c.Ancestor .Equals(nodeId) && c.Descendant.Equals(parentId), cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException("Parent Invalid.");
        }

        var node = queryable.FirstOrDefault(c => c.Ancestor.Equals(nodeId) && c.Descendant.Equals(nodeId));
        Debug.Assert(node == null || node.Distance == 0);
        if (node == null)
        {
            // 新建节点
            node = new TRelation() {
                ProviderType = providerType,
                ProviderName = providerName,
                ProviderKey = providerKey,
                Ancestor = nodeId,
                Descendant = nodeId,
                Distance = 0,
                IsRoot = false,
            };
            await InsertAsync(node, true, cancellationToken);
        }

        await DeleteAncestorRelationsAsync(providerType, providerName, providerKey, nodeId, cancellationToken);
        await CreateRelation(providerType,providerName,providerKey, nodeId, parentId.Value, queryable, autoSave, cancellationToken);
    }

    /// <summary>
    /// 删除节点的所有祖先关系
    /// </summary>
    /// <param name="providerType"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="nodeId"></param>
    /// <param name="cancellationToken"></param>
    private async Task DeleteAncestorRelationsAsync(string providerType, string providerName, string providerKey,
        TKey nodeId, CancellationToken cancellationToken)
    {
        var queryable = await GetQueryableAsync(providerType, providerName, providerKey);
        // 当前节点与子孙节点的关系
        var dRelations = queryable.Where(c => c.Ancestor.Equals(nodeId));
        // 自身及所有子孙节点Id
        var dIds = dRelations.Select(c => c.Descendant);

        // 所有原祖先节点Id
        var aIds = queryable
                .Where(c => c.Descendant.Equals(nodeId) && c.Distance > 0)
                .Select(c => c.Ancestor)
            ;

        // 删除`祖先`与`自身及子孙`之间的关系
        await DeleteAsync(
            c => c.ProviderType == providerType && c.ProviderName == providerName && c.ProviderKey == providerKey &&
                 dIds.Contains(c.Descendant) && aIds.Contains(c.Ancestor), false, cancellationToken);
    }

    private async Task CreateRootNodeAsync(string providerType, string providerName, string providerKey, TKey nodeId,
        bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync(providerType, providerName, providerKey);
        var node = queryable.FirstOrDefault(c => c.Ancestor.Equals(nodeId) && c.Descendant.Equals(nodeId));
        Debug.Assert(node == null || node.Distance == 0);
        if (node == null)
        {
            // 新建节点
            node = new TRelation() {
                ProviderType = providerType,
                ProviderName = providerName,
                ProviderKey = providerKey,
                Ancestor = nodeId,
                Descendant = nodeId,
                Distance = 0,
                IsRoot = true,
            };
            await InsertAsync(node, autoSave, cancellationToken);
        }
        else
        {
            await DeleteAncestorRelationsAsync(providerType, providerName, providerKey, nodeId, cancellationToken);
            node.IsRoot = true;
            await UpdateAsync(node, autoSave, cancellationToken);
        }
    }

    /// <summary>
    /// 重建`父节点`与`自身及子孙节点`的关系
    /// </summary>
    /// <param name="providerType"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="nodeId"></param>
    /// <param name="parentId"></param>
    /// <param name="queryable"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>

    private async Task CreateRelation(string providerType, string providerName, string providerKey,TKey nodeId, TKey parentId, IQueryable<TRelation> queryable, bool autoSave,
        CancellationToken cancellationToken)
    {
        // 新父节点的祖先关系
        var aRelations = queryable.Where(c => c.Descendant.Equals(parentId));
        var newRelations = new List<TRelation>();
        foreach (var dRelation in queryable.Where(c => c.Ancestor.Equals(nodeId)))
        {
            foreach (var aRelation in aRelations)
            {
                var relation = new TRelation() {
                    Ancestor = aRelation.Ancestor,
                    Descendant = dRelation.Descendant,
                    Distance = aRelation.Distance + dRelation.Distance + 1,
                    ProviderType = providerType,
                    ProviderName = providerName,
                    ProviderKey = providerKey,
                    IsRoot = false
                };
                newRelations.Add(relation);
            }
        }

        await InsertManyAsync(newRelations, autoSave, cancellationToken);
    }

    public async Task<TKey[]> DeleteAsync(string providerType, string providerName, string providerKey,
        TKey? nodeIdOrAll, bool includeDescendants = true, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        if (includeDescendants)
        {
            // 自身及子孙Id
            var dIds = (await GetDescendantsAsync(providerType, providerName, providerKey, nodeIdOrAll, null, true,
                cancellationToken: cancellationToken)).ToArray();

            await DeleteAsync(c =>
                c.ProviderType == providerType && c.ProviderName == providerName &&
                c.ProviderKey == providerKey
                && (dIds.Contains(c.Ancestor) || dIds.Contains(c.Descendant)), autoSave, cancellationToken);
            return dIds;
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}