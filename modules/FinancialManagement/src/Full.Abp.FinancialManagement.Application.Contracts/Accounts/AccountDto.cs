namespace Full.Abp.FinancialManagement.Accounts;

public class AccountDto
{
    public Guid Id { get; set; }
    public string ProviderName { get; set; }
    public string ProviderKey { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
    
    public bool IsEnabled { get; set; }
    public string DisplayName { get; set; }
}