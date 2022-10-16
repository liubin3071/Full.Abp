namespace Full.Abp.FinancialManagement.Accounts;

public class SystemAccountDecreaseInput
{
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public string Comments { get; set; }
}