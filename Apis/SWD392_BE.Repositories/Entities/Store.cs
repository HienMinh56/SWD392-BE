using System;
using System.Collections.Generic;

namespace SWD392_BE.Repositories.Entities;

public partial class Store : BaseEntity
{
    public string StoreId { get; set; } = null!;

    public string AreaId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int Status { get; set; }

    public int Phone { get; set; }

    public virtual Area Area { get; set; } = null!;

    public virtual ICollection<Food> Foods { get; } = new List<Food>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
