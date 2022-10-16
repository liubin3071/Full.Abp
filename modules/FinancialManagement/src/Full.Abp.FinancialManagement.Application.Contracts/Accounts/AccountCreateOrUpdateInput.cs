namespace Full.Abp.FinancialManagement.Accounts;

public class AccountCreateOrUpdateInput
{
    public string Name { get; set; }
    public string ProviderName { get; set; }
    public string ProviderKey { get; set; }
    public bool IsEnabled { get; set; } = true;
}