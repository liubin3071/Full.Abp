namespace Full.Abp.PaymentManagement.Payments;

public enum ChargeState
{
    /// <summary>
    /// 待支付
    /// </summary>
    Pending,
    /// <summary>
    /// 已支付
    /// </summary>
    Paid,
    /// <summary>
    /// 已取消(仅待支付可取消)
    /// </summary>
    Cancelled,
    /// <summary>
    /// 已退款(包括部分退款)
    /// </summary>
    Refunded
}