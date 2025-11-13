using System;
using System.Collections.Generic;

namespace Dormitory.Models.Entities;

public partial class Violation
{
    public string Violationid { get; set; } = null!;

    public string? Userid { get; set; }

    public string Violationtype { get; set; } = null!;

    public DateOnly? Violationdate { get; set; }

    public decimal? Penaltyfee { get; set; }

    public virtual User? User { get; set; }
}
