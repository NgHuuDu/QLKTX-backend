using System;
using System.Collections.Generic;

namespace Dormitory.Models.Entities;

public partial class Building
{
    public string Buildingid { get; set; } = null!;

    public string Buildingname { get; set; } = null!;

    public int Numberofrooms { get; set; }

    public int? Currentoccupancy { get; set; }

    public string? Gender { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
