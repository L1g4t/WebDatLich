using System;
using System.Collections.Generic;

namespace WebDatLich.Data;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Position { get; set; }

    public DateOnly? HireDate { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<TourGuide> TourGuides { get; set; } = new List<TourGuide>();
}
