using Full.Abp.Finance.Accounts;
using Volo.Abp.DependencyInjection;

namespace Full.Abp.FinancialManagement.Accounts;

public class AccountStore : IAccountStore, ITransientDependency
{
    private readonly AccountManager _accountManager;

    public AccountStore(AccountManager accountManager)
    {
        _accountManager = accountManager;
    }

    public virtual Task<decimal> GetBalanceAsync(string providerName, string providerKey, string name)
    {
        return _accountManager.GetBalanceAsync(providerName, providerKey, name);
    }

    public virtual Task<IDictionary<string, decimal>> GetBalancesAsync(string providerName, string providerKey, string[] names)
    {
        return _accountManager.GetBalancesAsync(providerName, providerKey, names);
    }

    public Task CreateAsync(string providerName, string providerKey, string name)
    {
        return _accountManager.CreateAsync(providerName, providerKey, name);
    }

    public virtual Task<Guid> IncreaseAsync(string providerName, string providerKey, string name, decimal amount, string transactionType,
        string transactionId, string? comments = null)
    {
        return _accountManager.IncreaseAsync(providerName, providerKey, name, amount, transactionType, transactionId,
            comments);
    }

    public virtual Task<Guid> DecreaseAsync(string providerName, string providerKey, string name, decimal amount, string transactionType,
        string transactionId, string? comments = null)
    {
        return _accountManager.DecreaseAsync(providerName, providerKey, name, amount, transactionType, transactionId,
            comments);
    }
}