using System;
using System.Collections.Generic;

namespace SWD392_BE.Repositories.Entities;

public partial class Area : BaseEntity
{
    public string AreaId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Status { get; set; }

    public virtual ICollection<AreaSession> AreaSessions { get; } = new List<AreaSession>();

    public virtual ICollection<Campus> Campuses { get; } = new List<Campus>();

    public virtual ICollection<Store> Stores { get; } = new List<Store>();
}
