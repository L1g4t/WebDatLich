﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;

namespace WebDatLich.Models
{
    public class EditTourViewModel
    {
        public int TourId { get; set; }

        public string TourName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public DateOnly? StartDay { get; set; }

        public DateOnly? EndDay { get; set; }

        public int DestinationId { get; set; }

        public List<SelectListItem> Destinations { get; set; } = new List<SelectListItem>();
    }

}
