using System;
using Volo.Abp.Domain.Entities;

namespace Full.Abp.PaymentManagement.Payments;

/// <summary>
/// 支付网关
/// </summary>
public class PaymentGateway : Entity<Guid>, IPaymentGateway
{
    public string GatewayName { get; set; }

    public string? ServiceProviderId { get; set; }

    public string MerchantId { get; set; }

    public string? SubMerchantId { get; set; }
}