namespace Full.Abp.FinancialManagement.Accounts;

public class AccountGetInput
{
    public string Name { get; set; }

    public string ProviderName { get; set; }
    
    public string ProviderKey { get; set; }
}