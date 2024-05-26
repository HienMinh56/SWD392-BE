using System;
using System.Collections.Generic;

namespace SWD392_BE.Repositories.Entities;

public partial class Order : BaseEntity
{
    public string OrderId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string AreaSessionId { get; set; } = null!;

    public int Price { get; set; }

    public int Quantity { get; set; }

    public string StoreId { get; set; } = null!;

    public string TransationId { get; set; } = null!;

    public int Status { get; set; }

    public virtual AreaSession AreaSession { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual Store Store { get; set; } = null!;

    public virtual Transaction Transation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
