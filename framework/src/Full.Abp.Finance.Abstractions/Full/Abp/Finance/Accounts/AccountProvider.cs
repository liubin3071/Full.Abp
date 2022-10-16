namespace Full.Abp.Finance.Accounts;



public abstract class AccountProvider : IAccountProvider
{
    protected IAccountStore AccountStore { get; }

    protected abstract string Name { get; }
    protected abstract string ProviderKey { get; }

    protected AccountProvider(IAccountStore accountStore)
    {
        AccountStore = accountStore;
    }

    public Task<decimal> GetBalanceAsync(string name)
    {
        return AccountStore.GetBalanceAsync(Name, ProviderKey, name);
    }

    public Task<IDictionary<string, decimal>> GetBalancesAsync(string[] names)
    {
        return AccountStore.GetBalancesAsync(Name, ProviderKey, names);
    }

    public Task CreateAsync(string name)
    {
        return AccountStore.CreateAsync(Name, ProviderKey, name);
    }


    public Task<Guid> IncreaseAsync(string name, decimal amount, string transactionType, string transactionId,
        string? comments = null)
    {
        return AccountStore.IncreaseAsync(Name, ProviderKey, name, amount, transactionType, transactionId,
            comments);
    }

    public Task<Guid> DecreaseAsync(string name, decimal amount, string transactionType, string transactionId,
        string? comments = null)
    {
        return AccountStore.DecreaseAsync(Name, ProviderKey, name, amount, transactionType, transactionId,
            comments);
    }
}