namespace Full.Abp.PaymentManagement.Payments;

public class PaymentGatewayInfo:IPaymentGateway
{
    public string Name { get; set; }

    public string? ServiceProviderId { get; set; }

    public string MerchantId { get; set; }

    public string? SubMerchantId { get; set; }
}

public interface IPaymentGateway
{
    public string Name { get; }

    public string? ServiceProviderId { get; }

    public string MerchantId { get; }

    public string? SubMerchantId { get; }
}