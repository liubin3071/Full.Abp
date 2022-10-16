namespace Full.Abp.FinancialManagement.Accounts;

public class AccountDecreaseInput
{
    public string Name { get; set; }
    public string ProviderName { get; set; }
    public string ProviderKey { get; set; }
    public decimal Amount { get; set; }
    public string Comments { get; set; }
}