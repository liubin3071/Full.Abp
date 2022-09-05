using System;
using System.Collections.Generic;
using Full.Abp.PaymentManagement.Payments;
using Volo.Abp.Domain.Entities;

namespace Full.Abp.PaymentManagement.Orders;

public class SimpleOrder : AggregateRoot<Guid>
{
    public Guid? PaymentId { get; set; }

    public string Title { get; set; }

    public string? Descriptionn { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; }
}

public class Order : AggregateRoot<Guid>
{
    public ICollection<OrderLine> OrderLines { get; set; }

    // /// <summary>
    // /// 优惠
    // /// </summary>
    // public ICollection<Discount> Discounts { get; set; }

    public Guid? PaymentId { get; set; }

    public string Title { get; set; }

    public string? Descriptionn { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; }
}