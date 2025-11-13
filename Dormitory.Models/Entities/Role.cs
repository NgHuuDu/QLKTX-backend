using System;
using System.Collections.Generic;

namespace Dormitory.Models.Entities;

public partial class Role
{
    public string Roleid { get; set; } = null!;

    public string Rolename { get; set; } = null!;

    public string? Roledescription { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
