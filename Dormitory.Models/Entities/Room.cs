using System;
using System.Collections.Generic;

namespace Dormitory.Models.Entities;

public partial class Room
{
    public string Roomid { get; set; } = null!;

    public int Roomnumber { get; set; }

    public string Buildingid { get; set; } = null!;

    public int? Capacity { get; set; }

    public int? Currentoccupancy { get; set; }

    public virtual Building Building { get; set; } = null!;

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}
