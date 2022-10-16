namespace Full.Abp.Finance.Accounts;

public interface IAccountProvider
{
    Task<decimal> GetBalanceAsync(string name);

    Task<IDictionary<string, decimal>> GetBalancesAsync(string[] names);

    Task CreateAsync(string name);
    
    Task<Guid> IncreaseAsync(string name, decimal amount, string transactionType, string transactionId,
        string? comments = null);

    Task<Guid> DecreaseAsync(string name, decimal amount, string transactionType, string transactionId,
        string? comments = null);
}