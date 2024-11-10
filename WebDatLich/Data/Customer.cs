using System;
using System.Collections.Generic;

namespace WebDatLich.Data;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
