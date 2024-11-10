using System;
using System.Collections.Generic;

namespace WebDatLich.Data;

public partial class Booking
{
    public int BookingId { get; set; }

    public int CustomerId { get; set; }

    public int TourId { get; set; }

    public DateOnly? BookingDate { get; set; }

    public string? Status { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Tour Tour { get; set; } = null!;
}
