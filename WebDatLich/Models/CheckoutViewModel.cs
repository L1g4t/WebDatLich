using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebDatLich.Models
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "vui long nhap ho ten khach hang")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "vui long nhap email khach hang")]
        public string Email { get; set; }

        [Required(ErrorMessage = "vui long nhap sdt khach hang")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "vui long nhap dia chi khach hang")]
        public string Address { get; set; }

        public int TourId { get; set; }
        public decimal TotalPrice { get; set; }

        public string PaymentMethod { get; set; }

        public List<SelectListItem> Customers { get; set; } = new();
        public List<SelectListItem> Bookings { get; set; } = new();
        public List<SelectListItem> Payment { get; set; } = new();

        
    }

}