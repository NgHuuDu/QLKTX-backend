using System;
using System.Collections.Generic;

namespace Dormitory.Models.Entities;

public partial class Payment
{
    public string Paymentid { get; set; } = null!;

    public string? Contractid { get; set; }

    public DateOnly Paymentdate { get; set; }

    public string? Paymentmethod { get; set; }

    public string? Paymentstatus { get; set; }

    public virtual Contract? Contract { get; set; }
}
