using System;
using System.Collections.Generic;

namespace SWD392_BE.Repositories.Entities;

public partial class Campus : BaseEntity
{
    public string CampusId { get; set; } = null!;

    public string AreaId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Status { get; set; }

    public virtual Area Area { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();
}
