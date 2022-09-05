using System;
using System.Collections.Generic;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Full.Abp.PaymentManagement.Payments;

public class Payment : AggregateRoot<Guid>, IMultiTenant, IHasCreationTime
{
    public Guid? TenantId { get; set; }
    public Guid? UserId { get; set; }
    public string Title { get; set; }

    public string? Descriptionn { get; set; }

    public decimal Amount { get; set; }

    public PaymentChannel Channel { get; set; }
    public Guid ChannelId { get; set; }

    /// <summary>
    /// 支付渠道应用场景业务Id(如:订单号)
    /// </summary>
    public string ChannelTransactionId { get; set; }

    public PaymentGatewayTransactionInfo? GatewayTransaction { get; set; }

    public ICollection<Refund> Refunds { get; set; }

    public ChargeState State { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime PaidTime { get; set; }

    public DateTime CancelTime { get; set; }

    public DateTime RefundedTime { get; set; }
}