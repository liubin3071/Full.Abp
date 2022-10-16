using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Full.Abp.Finance.Accounts;

public class UserAccountProvider : AccountProvider, ITransientDependency
{
    public const string ProviderName = "U";
    private readonly ICurrentUser _currentUser;
    protected override string Name => ProviderName;
    protected override string ProviderKey => _currentUser.Id.ToString();

    public UserAccountProvider(IAccountStore accountStore, ICurrentUser currentUser) : base(accountStore)
    {
        _currentUser = currentUser;
    }
}