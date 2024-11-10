using System;
using System.Collections.Generic;

namespace WebDatLich.Data;

public partial class TourGuide
{
    public int GuideId { get; set; }

    public int EmployeeId { get; set; }

    public int? ExperienceYears { get; set; }

    public string? LanguageSpoken { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
