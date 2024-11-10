using System;
using System.Collections.Generic;

namespace WebDatLich.Data;

public partial class TourDestination
{
    public int TourDestinationId { get; set; }

    public int TourId { get; set; }

    public int DestinationId { get; set; }

    public virtual Destination Destination { get; set; } = null!;

    public virtual Tour Tour { get; set; } = null!;
}
