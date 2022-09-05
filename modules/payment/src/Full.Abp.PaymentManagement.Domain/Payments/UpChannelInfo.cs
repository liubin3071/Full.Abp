namespace Full.Abp.PaymentManagement.Payments;

public class PaymentGatewayTransactionInfo : IPaymentGateway
{
    public string GatewayName { get; set; }

    public string? ServiceProviderId { get; set; }

    public string MerchantId { get; set; }

    public string? SubMerchantId { get; set; }

    public string TransactionId { get; set; }
}
 

public interface IPaymentGateway
{
    public string GatewayName { get; }

    public string? ServiceProviderId { get; }

    public string MerchantId { get; }

    public string? SubMerchantId { get; }
}