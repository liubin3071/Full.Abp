using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Full.Abp.PaymentManagement.Payments;

/// <summary>
/// 退款记录
/// </summary>
public class Refund : Entity<Guid>, IMultiTenant, IHasCreationTime, IHasConcurrencyStamp
{
    public Guid? TenantId { get; set; }
    public Guid? UserId { get; set; }
    public Guid PaymentId { get; set; }

    /// <summary>
    /// 退款金额
    /// </summary>
    public decimal Amount { get; set; }

    public DateTime CreationTime { get; set; }

    public string ConcurrencyStamp { get; set; }
}