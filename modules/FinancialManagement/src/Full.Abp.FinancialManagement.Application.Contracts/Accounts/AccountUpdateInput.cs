namespace Full.Abp.FinancialManagement.Accounts;

public class AccountUpdateInput
{
    public bool IsEnabled { get; set; }
    public decimal Amount { get; set; }
    public string? Comments { get; set; }
}