namespace Full.Abp.PaymentManagement.Orders;

public class OrderLine
{
    public string GoodsTitle { get; set; }

    public int Count { get; set; }

    public decimal Price { get; set; }
}