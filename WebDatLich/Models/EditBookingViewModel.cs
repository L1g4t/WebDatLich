using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebDatLich.Models
{
    public class EditBookingViewModel
    {
        public int BookingId { get; set; }
        public DateOnly? BookingDate { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; }

        public int TourId { get; set; }

        public List<SelectListItem> Tours { get; set; } = new();

        public int CustomerId { get; set; }

        public List<SelectListItem> Customers { get; set; } = new();
    }
}
