using System;
using System.Collections.Generic;

namespace Dormitory.Models.Entities;

public partial class Contract
{
    public string Contractid { get; set; } = null!;

    public string Userid { get; set; } = null!;

    public string Roomid { get; set; } = null!;

    public DateOnly Starttime { get; set; }

    public DateOnly Endtime { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Room Room { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
