using System;
using System.Collections.Generic;

namespace SWD392_BE.Repositories.Entities;

public partial class Session : BaseEntity
{
    public string SessionId { get; set; } = null!;

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public virtual ICollection<AreaSession> AreaSessions { get; } = new List<AreaSession>();
}
