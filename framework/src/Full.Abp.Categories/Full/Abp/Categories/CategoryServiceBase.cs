using System.Linq.Expressions;

namespace Full.Abp.Categories;

public abstract class CategoryServiceBase : ICategoryService
{
    public ICategoryServiceFactory CategoryServiceFactory { get; }
    protected abstract string ProviderKey { get; }

    protected abstract string ProviderName { get; }

    protected ICategoryService CategoryService => CategoryServiceFactory.Create(ProviderName, ProviderKey);

    protected CategoryServiceBase(ICategoryServiceFactory categoryServiceFactory)
    {
        CategoryServiceFactory = categoryServiceFactory;
    }

    public Task<CategoryInfo> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return CategoryService.GetAsync(id, cancellationToken);
    }

    public Task<CategoryInfo?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return CategoryService.FindAsync(id, cancellationToken);
    }

    public Task<List<CategoryInfo>> GetAncestorsAsync(string definitionName, Guid nodeId,
        CancellationToken cancellationToken = default)
    {
        return CategoryService.GetAncestorsAsync(definitionName, nodeId, cancellationToken);
    }

    public Task<List<CategoryInfo>> GetDescendantsAsync(string definitionName, Guid? nodeId, int? maxDistance = null,
        CancellationToken cancellationToken = default)
    {
        return CategoryService.GetDescendantsAsync(definitionName, nodeId, maxDistance, cancellationToken);
    }

    public Task<List<TreeNodeWrapper<CategoryInfo>>> GetTreeAsync(string definitionName, Guid? nodeId,
        CancellationToken cancellationToken = default)
    {
        return CategoryService.GetTreeAsync(definitionName, nodeId, cancellationToken);
    }

    public Task<bool> HasChildrenAsync(string definitionName, Guid? nodeId,
        CancellationToken cancellationToken = default)
    {
        return CategoryService.HasChildrenAsync(definitionName, nodeId, cancellationToken);
    }

    public Task<List<CategoryInfo>> GetChildrenAsync(string definitionName, Guid? nodeId,
        CancellationToken cancellationToken = default)
    {
        return CategoryService.GetChildrenAsync(definitionName, nodeId, cancellationToken);
    }

    public Task<List<CategoryInfo>> GetChildrenAsync(string definitionName, Guid? nodeId, int skipCount,
        int maxResultCount, string? sorting = null,
        CancellationToken cancellationToken = default)
    {
        return CategoryService.GetChildrenAsync(definitionName, nodeId, skipCount, maxResultCount, sorting,
            cancellationToken);
    }

    public Task<int> GetChildrenCountAsync(string definitionName, Guid? nodeId,
        CancellationToken cancellationToken = default)
    {
        return CategoryService.GetChildrenCountAsync(definitionName, nodeId, cancellationToken);
    }

    public Task<CategoryInfo> GetRootAsync(string definitionName, Guid nodeId,
        CancellationToken cancellationToken = default)
    {
        return CategoryService.GetRootAsync(definitionName, nodeId, cancellationToken);
    }

    public Task<CategoryInfo?> GetParentAsync(string definitionName, Guid nodeId,
        CancellationToken cancellationToken = default)
    {
        return CategoryService.GetParentAsync(definitionName, nodeId, cancellationToken);
    }

    public Task<CategoryInfo> CreateAsync(string definitionName, CategoryInfo entity, Guid? parentId = default,
        CancellationToken cancellationToken = default)
    {
        return CategoryService.CreateAsync(definitionName, entity, parentId, cancellationToken);
    }

    public Task EnsureParentAsync(string definitionName, Guid id, Guid? parentId,
        CancellationToken cancellationToken = default)
    {
        return CategoryService.EnsureParentAsync(definitionName, id, parentId, cancellationToken);
    }

    public Task DeleteAsync(string definitionName, Guid id, CancellationToken cancellationToken = default)
    {
        return CategoryService.DeleteAsync(definitionName, id, cancellationToken);
    }

    public Task DeleteAllAsync(string definitionName, CancellationToken cancellationToken = default)
    {
        return CategoryService.DeleteAllAsync(definitionName, cancellationToken);
    }
}