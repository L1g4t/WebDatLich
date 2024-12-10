using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatLich.Data;
using WebDatLich.Models;

namespace WebDatLich.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CsdlDuLichContext _context;

        public CheckoutController(CsdlDuLichContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            var tour = _context.Tours.Include(t => t.Destination).FirstOrDefault(t => t.TourId == id);
            return View(tour);
        }

        [HttpGet]
        public IActionResult CheckOutForm(int id)
        {
            var account = HttpContext.Session.GetString("Username");
            var tour = _context.Tours.FirstOrDefault(t => t.TourId == id);
            var customers = _context.Customers
                .Include(b => b.Accounts)
                .Where(b => b.Accounts.Any(a => a.Username == account))
                .Select(s => new CheckoutViewModel
                {
                    FullName = s.FullName,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address,
                    TourId = id,
                    TotalPrice = tour.Price,
                })
                .FirstOrDefault();
            return View(customers);
        }

        [HttpPost]
        public async Task<IActionResult> BookTour(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = HttpContext.Session.GetString("Username");
                var customer = await _context.Customers
                    .Include(c => c.Accounts)
                    .FirstOrDefaultAsync(c => c.Accounts.Any(a => a.Username == account));

                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                var booking = new Booking
                {
                    CustomerId = customer.CustomerId,
                    TourId = model.TourId,
                    BookingDate = DateOnly.FromDateTime(DateTime.Now),
                    Status = "Confirmed",
                    TotalPrice = model.TotalPrice
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Booking successful!";
                return RedirectToAction("BookingSuccess");
            }

            return View("CheckOutForm", model);
        }

        [HttpGet]
        public IActionResult BookingSuccess()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View();
        }
    }
}
