using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Full.Abp.Trees.EntityFrameworkCore;

public abstract class EfCoreTreeRepositoryBase<TDbContext, TTreeEntity, TTree, TRelation, TKey>
    : EfCoreRepository<TDbContext, TTreeEntity, TKey>, ITreeRepository<TTreeEntity, TKey>
    where TDbContext : IEfCoreDbContext
    where TTreeEntity : class, IEntity<TKey>
    where TTree : Tree<TKey>, new()
    where TRelation : TreeNodeRelation<TKey>, new()
{
    protected virtual async Task<IQueryable<TTree>> GetTreeQueryableAsync()
    {
        return (await GetDbContextAsync()).Set<TTree>().AsQueryable();
    }

    protected virtual async Task<IQueryable<TRelation>> GetRelationQueryableAsync()
    {
        return (await GetDbContextAsync()).Set<TRelation>().AsQueryable();
    }

    protected virtual Task<IQueryable<TTreeEntity>> GetQueryableAsync(bool includeDetails)
    {
        return includeDetails ? WithDetailsAsync() : GetQueryableAsync();
    }

    protected virtual async Task<List<TTreeEntity>> GetEntitiesByIdsAsync(IEnumerable<TKey> idsQueryable,
        bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(includeDetails))
            .Where(c => idsQueryable.Contains(c.Id))
            .ToListAsync(cancellationToken: cancellationToken);
    }

    protected virtual async Task DeleteRelationAsync(Expression<Func<TRelation, bool>> predicate, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbset = (await GetDbContextAsync())
            .Set<TRelation>();
        var relations = await dbset
            .Where(predicate)
            .ToListAsync(GetCancellationToken(cancellationToken));

        dbset.RemoveRange(relations);

        if (autoSave)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }

    protected virtual async Task DeleteRelationWithoutChildrenAsync(TKey id, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var childrenRelations = await (await GetRelationQueryableAsync())
            .Where(c => c.Ancestor.Equals(id) && c.Distance > 0)
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var relation in childrenRelations)
        {
            relation.Distance--;
        }

        (await GetDbContextAsync()).Set<TRelation>().UpdateRange(childrenRelations);
        var selfRelation = await (await GetRelationQueryableAsync())
            .FirstAsync(c => c.Ancestor.Equals(id) || c.Descendant.Equals(id), cancellationToken: cancellationToken);
        (await GetDbContextAsync()).Set<TRelation>().Remove(selfRelation);
        if (autoSave)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }

    protected EfCoreTreeRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task EnsureCreateTree(string providerType, string providerName, string providerKey)
    {
        var exist = (await GetTreeQueryableAsync()).Any(c =>
            c.ProviderType == providerType && c.ProviderName == providerName && c.ProviderKey == providerKey);
        if (exist)
        {
            return;
        }

        var tree = await (await GetDbContextAsync()).Set<TTree>().AddAsync(new TTree() {
            ProviderName = providerName, ProviderKey = providerKey, ProviderType = providerType
        });
        await (await GetDbContextAsync()).Set<TRelation>().AddAsync(new TRelation() {
            Ancestor = tree.Entity.Id, Descendant = tree.Entity.Id, Distance = 0, TreeId = tree.Entity.Id
        });
    }

    public virtual async Task<TKey> GetTreeIdAsync(string providerType, string providerName, string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await (await GetTreeQueryableAsync())
            .Where(tree => tree.ProviderType == providerType && tree.ProviderName == providerName &&
                           tree.ProviderKey == providerKey)
            .Select(tree => tree.Id)
            .FirstAsync(cancellationToken);
    }
    public virtual async Task<List<TreeNodeWrapper<TTreeEntity>>> GetTreeAsync(string providerType, string providerName,
        string providerKey,
        bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var treeId = await GetTreeIdAsync(providerType, providerName, providerKey, cancellationToken);
        var ids = (await GetRelationQueryableAsync())
            .Where(c => c.Ancestor.Equals(treeId) && c.Distance > 0)
            .Select(c => c.Descendant);


        var r = await (from id in ids
                join entity in (await GetQueryableAsync()) on id equals entity.Id
                join relation in (await GetRelationQueryableAsync()).Where(r => r.Distance == 1) on id equals relation.Descendant
                select new { Value = entity, ParentId = relation.Ancestor })
            .ToListAsync(cancellationToken: cancellationToken);

        return r.ToTree(src => src.ParentId, dst => dst.Value.Id,
            src => new TreeNodeWrapper<TTreeEntity>() { Value = src.Value }, treeId);
    }
    public virtual async Task<List<TTreeEntity>> GetAllAsync(string providerType, string providerName,
        string providerKey,
        bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var treeId = await GetTreeIdAsync(providerType, providerName, providerKey, cancellationToken);
        var ids = (await GetRelationQueryableAsync())
            .Where(c => c.Ancestor.Equals(treeId) && c.Distance > 0)
            .Select(c => c.Descendant);
        return await GetEntitiesByIdsAsync(ids, includeDetails, cancellationToken);
    }

    public Task<List<TTreeEntity>> GetListByIdsAsync(IEnumerable<TKey> ids, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return GetEntitiesByIdsAsync(ids, includeDetails, cancellationToken);
    }

    public virtual async Task<List<TTreeEntity>> GetAncestorsAsync(TKey id, bool includeSelf = false,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var idsQueryable = (await GetRelationQueryableAsync())
            .Where(c => c.Descendant.Equals(id))
            .WhereIf(!includeSelf, c=>c.Distance > 0)
            .Select(c => c.Ancestor);
        return await GetEntitiesByIdsAsync(idsQueryable, includeDetails, cancellationToken);
    }

    public virtual async Task<TTreeEntity> GetAncestorAsync(TKey id, int distance, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var ancestorId = await (await GetRelationQueryableAsync())
            .Where(c => c.Descendant.Equals(id) && c.Distance == distance)
            .Select(c => c.Ancestor)
            .FirstAsync(cancellationToken: cancellationToken);
        return await GetAsync(ancestorId, includeDetails, cancellationToken);
    }

    public virtual async Task<int> GetDeepinAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await (await GetRelationQueryableAsync())
            .Where(c => c.Descendant.Equals(id) && c.TreeId.Equals(c.Ancestor))
            .Select(c => c.Distance)
            .FirstAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<List<TTreeEntity>> GetDescendantsAsync(TKey id, int? maxDistance = null,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var descendantIds = (await GetRelationQueryableAsync())
            .Where(c => c.Ancestor.Equals(id))
            .WhereIf(maxDistance.HasValue, c => c.Distance <= maxDistance)
            .Select(c => c.Descendant);
        return await GetEntitiesByIdsAsync(descendantIds, includeDetails, cancellationToken);
    }


    public virtual async Task<bool> HasChildrenAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await (await GetRelationQueryableAsync())
            .AnyAsync(c => c.Ancestor.Equals(id) && c.Distance > 0, cancellationToken: cancellationToken);
    }

    public virtual async Task<int> GetChildrenCountAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await (await GetRelationQueryableAsync())
            .CountAsync(c => c.Ancestor.Equals(id) && c.Distance == 1, cancellationToken: cancellationToken);
    }

    public virtual async Task<List<TTreeEntity>> GetChildrenAsync(TKey id, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var ids = (await GetRelationQueryableAsync())
            .Where(c => c.Ancestor.Equals(id) && c.Distance == 1)
            .Select(c => c.Descendant);
        return await GetEntitiesByIdsAsync(ids, includeDetails, cancellationToken);
    }

    public virtual async Task<List<TTreeEntity>> GetPagedChildrenAsync(TKey id, int skipCount, int maxResultCount,
        string sorting, bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var ids = (await GetRelationQueryableAsync())
            .Where(c => c.Ancestor.Equals(id) && c.Distance == 1)
            .Select(c => c.Descendant);
        return await (await GetQueryableAsync(includeDetails))
            .Where(c => ids.Contains(c.Id))
            .OrderBy(sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async override Task<TTreeEntity> InsertAsync(TTreeEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var treeId = (await GetRelationQueryableAsync())
            .Where(c => c.Descendant.Equals(entity.Id))
            .OrderByDescending(c => c.Distance)
            .Select(c => c.Ancestor)
            .First();
        return await InsertAsync(entity, treeId, autoSave, cancellationToken);
    }

    public async Task<TTreeEntity> InsertAsync(TTreeEntity entity, TKey parentId, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        await base.InsertAsync(entity, autoSave, cancellationToken);
        if (entity.Id.Equals(default))
        {
            await SaveChangesAsync(cancellationToken);
        }

        Debug.Assert(entity.Id != null);
        // 创建自节点
        var parent = (await GetRelationQueryableAsync()).First(c => c.Ancestor.Equals(parentId) && c.Distance == 0);
        await (await GetDbContextAsync()).Set<TRelation>().AddAsync(
            new TRelation() { Ancestor = entity.Id, Descendant = entity.Id, Distance = 0, TreeId = parent.TreeId },
            cancellationToken);
        // 必须先保存当前自节点关系
        await SaveChangesAsync(cancellationToken);
        await UpdateParentRelationAsync(entity.Id, parentId, autoSave, cancellationToken);
        return entity;
    }

    public async Task InsertManyAsync(IEnumerable<TTreeEntity> entities, TKey parentId, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var treeEntities = entities as TTreeEntity[] ?? entities.ToArray();
        await base.InsertManyAsync(treeEntities, autoSave, cancellationToken);
        if (treeEntities.Any(c => c.Id.Equals(default)))
        {
            await SaveChangesAsync(cancellationToken);
        }

        foreach (var entity in treeEntities)
        {
            await (await GetDbContextAsync()).Set<TRelation>().AddAsync(
                new TRelation() { Ancestor = entity.Id, Descendant = entity.Id, Distance = 0 }, cancellationToken);
            await UpdateParentRelationAsync(entity.Id, parentId, autoSave, cancellationToken);
        }

        if (autoSave)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task EnsureParentAsync(TKey id, TKey parentId, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        await UpdateParentRelationAsync(id, parentId, autoSave, cancellationToken);
    }

    private async Task UpdateParentRelationAsync(TKey id, TKey parentId, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var parent = (await GetRelationQueryableAsync()).First(c => c.Ancestor.Equals(parentId) && c.Distance == 0);
        
        // 所有子孙节点Id, 包括自身
        var dIds = (await GetRelationQueryableAsync())
            .Where(c => c.Ancestor.Equals(id))
            .Select(c => c.Descendant);

        // 所有原祖先节点Id
        var aIds = (await GetRelationQueryableAsync())
            .Where(c => c.Descendant.Equals(id) && c.Distance > 0)
            .Select(c => c.Ancestor);

        // 删除祖孙之间的关系
        // TODO BulkOperationProvider
        var relations =
            (await GetRelationQueryableAsync()).Where(c => dIds.Contains(c.Descendant) && aIds.Contains(c.Ancestor));
        (await GetDbContextAsync()).Set<TRelation>().RemoveRange(relations.ToList());

        // 当前节点与子孙节点的关系
        var dRelations = await (await GetRelationQueryableAsync())
            .Where(c => c.Ancestor.Equals(id))
            .ToListAsync(cancellationToken: cancellationToken);

        // 新父节点的祖先关系
        var aRelations = await (await GetRelationQueryableAsync())
            .Where(c => c.Descendant.Equals(parentId))
            .ToListAsync(cancellationToken: cancellationToken);

        var newRelations = new List<TRelation>();
        foreach (var dRelation in dRelations)
        {
            foreach (var aRelation in aRelations)
            {
                var relation = new TRelation() {
                    Ancestor = aRelation.Ancestor,
                    Descendant = dRelation.Descendant,
                    Distance = aRelation.Distance + dRelation.Distance + 1,
                    TreeId = parent.TreeId,
                };
                newRelations.Add(relation);
            }
        }

        await (await GetDbContextAsync()).Set<TRelation>().AddRangeAsync(newRelations, cancellationToken);
        if (autoSave)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }


    public virtual async Task DeleteWithoutDescendantsAsync(TKey id, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return;
        }

        await DeleteWithoutDescendantsAsync(entity, autoSave, cancellationToken);
    }


    public virtual async Task DeleteWithoutDescendantsAsync(TTreeEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        await base.DeleteAsync(entity, false, cancellationToken);
        await DeleteRelationWithoutChildrenAsync(entity.Id, false, cancellationToken);
        if (autoSave)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }

    public override Task DeleteAsync(TTreeEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        return DeleteManyAsync(new[] { entity }, autoSave, cancellationToken);
    }

    public virtual async Task DeleteWithoutDescendantsAsync(Expression<Func<TTreeEntity, bool>> predicate,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var dbSet = dbContext.Set<TTreeEntity>();

        var entities = await dbSet
            .Where(predicate)
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyWithoutDescendantAsync(entities, autoSave, cancellationToken);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }
    }

    public virtual async Task DeleteManyWithoutDescendantAsync(IEnumerable<TTreeEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var treeEntities = entities.ToList();
        await base.DeleteManyAsync(treeEntities, autoSave, cancellationToken);

        var delIds = treeEntities.Select(c => c.Id).ToList();
        foreach (var delId in delIds)
        {
            await DeleteRelationWithoutChildrenAsync(delId, true, cancellationToken);
        }

        if (autoSave)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }

    public async override Task DeleteManyAsync(IEnumerable<TTreeEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        // 获取所有后代Id
        var dIds = (await GetRelationQueryableAsync())
            .Where(c => entities.Select(e => e.Id).Contains(c.Ancestor))
            .Select(c => c.Descendant);
        // 删除所有关系
        await DeleteRelationAsync(c => dIds.Contains(c.Ancestor) || dIds.Contains(c.Descendant), autoSave,
            cancellationToken: cancellationToken);
        // 删除所有后代
        var dEntities = await GetListAsync(c => dIds.Contains(c.Id), false, cancellationToken);
        await base.DeleteManyAsync(dEntities, autoSave, cancellationToken);
    }

    public virtual async Task DeleteManyWithoutDescendantAsync(IEnumerable<TKey> ids, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var entities = await (await GetQueryableAsync())
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);
        await DeleteManyWithoutDescendantAsync(entities, autoSave, cancellationToken);
    }
}