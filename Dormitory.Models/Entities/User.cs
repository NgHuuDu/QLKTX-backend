using System;
using System.Collections.Generic;

namespace Dormitory.Models.Entities;

public partial class User
{
    public string Userid { get; set; } = null!;

    public string? Studentid { get; set; }

    public string? Roleid { get; set; }

    public string Username { get; set; } = null!;

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual Role? Role { get; set; }

    public virtual Student? Student { get; set; }

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
