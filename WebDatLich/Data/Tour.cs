using System;
using System.Collections.Generic;

namespace WebDatLich.Data;

public partial class Tour
{
    public int TourId { get; set; }

    public string TourName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public DateOnly? StartDay { get; set; }

    public DateOnly? EndDay { get; set; }

    public int DestinationId { get; set; }

    public int GuideId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Destination Destination { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual TourGuide Guide { get; set; } = null!;
}
