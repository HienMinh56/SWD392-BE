using System;
using System.Collections.Generic;

namespace SWD392_BE.Repositories.Entities;

public partial class AreaSession : BaseEntity
{
    public string AreaSessionId { get; set; } = null!;

    public string SessionId { get; set; } = null!;

    public string AreaId { get; set; } = null!;

    public virtual Area Area { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual Session Session { get; set; } = null!;
}
