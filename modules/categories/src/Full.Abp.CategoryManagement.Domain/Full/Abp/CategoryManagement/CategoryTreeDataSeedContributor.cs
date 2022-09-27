using System.Threading.Tasks;
using Full.Abp.Categories.Definitions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Full.Abp.CategoryManagement;

public class CategoryTreeDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly ICategoryDefinitionManager _categoryDefinitionManager;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentTenant _currentTenant;

    public CategoryTreeDataSeedContributor(ICategoryDefinitionManager categoryDefinitionManager,ICategoryRepository categoryRepository,
         ICurrentTenant currentTenant)
    {
        _categoryDefinitionManager = categoryDefinitionManager;
        _categoryRepository = categoryRepository;
        _currentTenant = currentTenant;
    }

    [UnitOfWork]
    public async Task SeedAsync(DataSeedContext context)
    {
        foreach (var categoryDefinition in _categoryDefinitionManager.GetAll())
        {
            var side = context.TenantId.HasValue ? MultiTenancySides.Tenant : MultiTenancySides.Host;
            if (!categoryDefinition.MultiTenancySides.HasFlag(side))
            {
                continue;
            }

            using var disposable = _currentTenant.Change(context.TenantId);
            var providerType = categoryDefinition.Name;
            var providerKey = context.TenantId.ToString();
            var providerName = context.TenantId.HasValue ? "T" : "G";
            await _categoryRepository.EnsureCreateTree(providerType, providerName, providerKey);
        }
    }
}