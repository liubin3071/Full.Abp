namespace Full.Abp.Finance.Accounts;

public interface IAccountStore
{
    Task<decimal> GetBalanceAsync(string providerName, string providerKey, string name);

    Task<IDictionary<string, decimal>> GetBalancesAsync(string providerName, string providerKey, string[] names);
    
    Task CreateAsync(string providerName, string providerKey, string name);

    Task<Guid> IncreaseAsync(string providerName,
        string providerKey, string name, decimal amount, string transactionType, string transactionId,
        string? comments = null);

    Task<Guid> DecreaseAsync(string providerName,
        string providerKey, string name, decimal amount, string transactionType, string transactionId,
        string? comments = null);
}