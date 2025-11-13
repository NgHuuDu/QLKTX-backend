using System;
using System.Collections.Generic;

namespace Dormitory.Models.Entities;

public partial class Student
{
    public string Studentid { get; set; } = null!;

    public string Studentname { get; set; } = null!;

    public string? Gender { get; set; }

    public string? Department { get; set; }

    public DateOnly? Dateofbirth { get; set; }

    public string? Phonenumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
