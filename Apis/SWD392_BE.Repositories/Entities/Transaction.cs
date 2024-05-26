using System;
using System.Collections.Generic;

namespace SWD392_BE.Repositories.Entities;

public partial class Transaction
{
    public int Id { get; set; }

    public string TransationId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public int Type { get; set; }

    public int Amonut { get; set; }

    public int Status { get; set; }

    public DateTime Time { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual User User { get; set; } = null!;
}
