using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Full.Abp.PaymentManagement.Payments;

/// <summary>
/// 支付场景渠道
/// </summary>
public class PaymentChannel : FullAuditedAggregateRoot<Guid>
{
    public Guid? ParentId { get; set; }
    public PaymentChannel? Parent { get; set; }

    public ICollection<PaymentChannel> Children { get; set; }

    public string Name { get; set; }

    public string Comments { get; set; }
}