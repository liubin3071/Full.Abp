using Full.Abp.Finance.Accounts;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Full.Abp.FinancialManagement.Accounts;

public class SystemAccountDataSeedContributor : IDataSeedContributor,ITransientDependency
{
    private readonly IAccountDefinitionManager _accountDefinitionManager;
    private readonly AccountManager _accountManager;
    private readonly ICurrentTenant _currentTenant;

    public SystemAccountDataSeedContributor(
        IAccountDefinitionManager accountDefinitionManager,
        AccountManager accountManager,
        ICurrentTenant currentTenant)
    {
        _accountDefinitionManager = accountDefinitionManager;
        _accountManager = accountManager;
        _currentTenant = currentTenant;
    }

    [UnitOfWork]
    public async Task SeedAsync(DataSeedContext context)
    {
        foreach (var definition in _accountDefinitionManager.GetAll())
        {
            using (_currentTenant.Change(context.TenantId))
            {
                if (!definition.MultiTenancySide.HasFlag(_currentTenant.GetMultiTenancySide()))
                {
                    continue;
                }

                var name = definition.Name;
                var providerKey = context.TenantId.ToString();
                var providerName = context.TenantId.HasValue ? TenantAccountProvider.ProviderName : GlobalAccountProvider.ProviderName;
                if (await _accountManager.FindIdAsync(providerName, providerKey, name) == null)
                {
                    await _accountManager.CreateAsync(providerName, providerKey, name);
                }
            }
        }
    }
}