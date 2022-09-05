namespace Full.Abp.PaymentManagement.Orders;

public class OrderLine
{
    public string GoodsTitle { get; set; }

    public decimal UnitPrice { get; set; }

    public int Count { get; set; }

    public decimal TotalPrice { get; set; }

    // /// <summary>
    // /// 优惠
    // /// </summary>
    // public ICollection<Discount> Discounts { get; set; }
}