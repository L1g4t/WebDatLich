using System;
using System.Collections.Generic;

namespace WebDatLich.Data;

public partial class Destination
{
    public int DestinationId { get; set; }

    public string DestinationName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<TourDestination> TourDestinations { get; set; } = new List<TourDestination>();
}
