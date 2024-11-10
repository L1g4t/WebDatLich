using System;
using System.Collections.Generic;

namespace WebDatLich.Data;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int CustomerId { get; set; }

    public int TourId { get; set; }

    public int? Rating { get; set; }

    public string? Comments { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Tour Tour { get; set; } = null!;
}
