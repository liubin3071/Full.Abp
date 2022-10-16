namespace Full.Abp.FinancialManagement.AccountEntries;

public class AccountEntryGetListOutput
{
    /// <summary>
    /// 账户Id
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// 在账户中的索引
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// 金额（可为负）.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 账户余额
    /// </summary>
    public decimal PostBalance { get; set; }

    public string TransactionType { get; set; } = null!;

    /// <summary>
    /// 业务/凭证Id
    /// </summary>
    public string TransactionId { get; set; }

    public string? Comments { get; set; }

    public DateTime CreationTime { get; set; }
}