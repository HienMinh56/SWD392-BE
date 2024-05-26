﻿using System;
using System.Collections.Generic;

namespace SWD392_BE.Repositories.Entities;

public partial class User : BaseEntity
{
    public string UserId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string CampusId { get; set; } = null!;

    public int Phone { get; set; }

    public int Role { get; set; }

    public int Balance { get; set; }

    public int Status { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public virtual Campus Campus { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();
}
