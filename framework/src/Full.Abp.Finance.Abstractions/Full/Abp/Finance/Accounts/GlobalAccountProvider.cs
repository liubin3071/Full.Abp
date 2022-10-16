using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Full.Abp.Finance.Accounts;

public class GlobalAccountProvider : AccountProvider, ITransientDependency
{
    public const string ProviderName = "G";
    private readonly ICurrentUser _currentUser;
    protected override string Name => ProviderName;
    protected override string ProviderKey => string.Empty;

    public GlobalAccountProvider(IAccountStore accountStore, ICurrentUser currentUser) : base(accountStore)
    {
        _currentUser = currentUser;
    }
}