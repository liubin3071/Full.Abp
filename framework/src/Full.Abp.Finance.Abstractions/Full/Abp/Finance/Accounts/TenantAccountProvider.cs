using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Full.Abp.Finance.Accounts;

public class TenantAccountProvider : AccountProvider, ITransientDependency
{
    public const string ProviderName = "T";
    private readonly ICurrentTenant _currentTenant;
    protected override string Name => ProviderName;
    protected override string ProviderKey => _currentTenant.Id.ToString();

    public TenantAccountProvider(IAccountStore accountStore, ICurrentTenant currentTenant) : base(accountStore)
    {
        _currentTenant = currentTenant;
    }
}