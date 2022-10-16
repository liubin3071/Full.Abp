using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Full.Abp.FinancialManagement.Accounts;

/// <summary>
/// 账簿条目
/// </summary>
public class AccountEntry : Entity<Guid>, IHasCreationTime, IHasExtraProperties
{
    protected AccountEntry()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }


    public AccountEntry(Guid accountId, int index, decimal amount, decimal postBalance, string transactionType,
        string transactionId, string? comments) : this()
    {
        AccountId = accountId;
        Index = index;
        Amount = amount;
        PostBalance = postBalance;
        TransactionType = transactionType;
        TransactionId = transactionId;
        Comments = comments;
    }


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

    public ExtraPropertyDictionary ExtraProperties { get; }
}