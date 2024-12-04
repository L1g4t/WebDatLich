using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebDatLich.Data;
using WebDatLich.Models;

namespace WebDatLich.Controllers
{
    public class AdminBookingController : Controller
    {
        private readonly CsdlDuLichContext _context;

        public AdminBookingController(CsdlDuLichContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Booking(string searchString, string sortOrder)
        {
            
            var bookingQuery = _context.Bookings
                .Include(t => t.Customer)
                .Include(t => t.Tour)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                bookingQuery = bookingQuery.Where(t => t.Tour.TourName.Contains(searchString) 
                || t.Customer.FullName.Contains(searchString) 
                || t.Customer.Email.Contains(searchString));
            }

            var tours = await bookingQuery.ToListAsync();
            return View(tours);
        }

        [HttpGet]
        public IActionResult AddBooking()
        {
            var customer = _context.Customers
                .Select(d => new SelectListItem
                {
                    Value = d.CustomerId.ToString(),
                    Text = d.FullName
                })
                .ToList();

            var tour = _context.Tours
                .Select(d => new SelectListItem
                {
                    Value = d.TourId.ToString(),
                    Text = d.TourName
                })
                .ToList();

            if (!customer.Any())
            {
                TempData["ErrorMessage"] = "Không có người dùng nào để thêm Booking. Vui lòng thêm người dùng trước.";
                return RedirectToAction("Account", "AdminAccount");
            }

            if (!tour.Any())
            {
                TempData["ErrorMessage"] = "Không có tour nào để thêm Booking. Vui lòng thêm tour trước.";
                return RedirectToAction("Tours", "Admin");
            }

            var model = new AdminBookingViewModel
            {
                Customers = customer,
                Tours = tour
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddBooking(AdminBookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Tours = _context.Tours
                    .Select(d => new SelectListItem
                    {
                        Value = d.TourId.ToString(),
                        Text = d.TourName
                    })
                    .ToList();
                model.Customers = _context.Customers
                    .Select(d => new SelectListItem
                    {
                        Value = d.CustomerId.ToString(),
                        Text = d.FullName
                    })
                    .ToList();

                return View(model);
            }

            var toursExists = await _context.Tours
                .AnyAsync(d => d.TourId == model.TourId);
            var customersExists = await _context.Customers
                .AnyAsync(d => d.CustomerId == model.CustomerId);

            if (!toursExists)
            {
                ModelState.AddModelError("TourId", "Tour không hợp lệ.");

                model.Tours = _context.Tours
                    .Select(d => new SelectListItem
                    {
                        Value = d.TourId.ToString(),
                        Text = d.TourName
                    })
                    .ToList();

                return View(model);
            }
            else if (!customersExists)
            {
                ModelState.AddModelError("CustomerId", "Người dùng không hợp lệ.");

                model.Customers = _context.Customers
                    .Select(d => new SelectListItem
                    {
                        Value = d.CustomerId.ToString(),
                        Text = d.FullName
                    })
                    .ToList();

                return View(model);
            }

            var booking = new Booking
            {
                BookingDate = model.BookingDate,
                TotalPrice = model.Price,
                Status = model.Status,
                TourId = model.TourId,
                CustomerId = model.CustomerId
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Thêm Booking thành công!";
            return RedirectToAction("Booking", "AdminBooking");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking không tồn tại.";
                return RedirectToAction("Booking", "AdminBooking");
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Xóa Booking thành công!";
            return RedirectToAction("Booking", "AdminBooking");
        }
        
        [HttpGet]
        public async Task<IActionResult> EditBooking(int id)
        {
            var booking = await _context.Bookings
                .Include(t => t.Customer)
                .Include(t => t.Tour)
                .FirstOrDefaultAsync(t => t.BookingId == id);

            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking không tồn tại.";
                return RedirectToAction("Booking", "AdminBooking");
            }

            var model = new AdminBookingViewModel
            {
                BookingId = booking.BookingId,
                BookingDate = booking.BookingDate,
                Status = booking.Status,
                TourId = booking.TourId,
                Tours = await _context.Tours
                    .Select(d => new SelectListItem
                    {
                        Value = d.TourId.ToString(),
                        Text = d.TourName
                    })
                    .ToListAsync(),
                CustomerId = booking.CustomerId,
                Customers = await _context.Customers
                    .Select(d=> new SelectListItem
                    {
                        Value= d.CustomerId.ToString(),
                        Text = d.FullName
                    })
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBooking(AdminBookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Tours =await _context.Tours
                    .Select(d => new SelectListItem
                    {
                        Value = d.TourId.ToString(),
                        Text = d.TourName
                    })
                    .ToListAsync();
                model.Customers =await _context.Customers
                    .Select(d => new SelectListItem
                    {
                        Value = d.CustomerId.ToString(),
                        Text = d.FullName
                    })
                    .ToListAsync();

                return View(model);
            }

            var toursExists = await _context.Tours
                .AnyAsync(d => d.TourId == model.TourId);
            var customersExists = await _context.Customers
                .AnyAsync(d => d.CustomerId == model.CustomerId);

            if (!toursExists)
            {
                ModelState.AddModelError("TourId", "Tour không hợp lệ.");

                model.Tours =await _context.Tours
                    .Select(d => new SelectListItem
                    {
                        Value = d.TourId.ToString(),
                        Text = d.TourName
                    })
                    .ToListAsync();

                return View(model);
            }
            else if (!customersExists)
            {
                ModelState.AddModelError("CustomerId", "Người dùng không hợp lệ.");

                model.Customers =await _context.Customers
                    .Select(d => new SelectListItem
                    {
                        Value = d.CustomerId.ToString(),
                        Text = d.FullName
                    })
                    .ToListAsync();

                return View(model);
            }

            var booking = await _context.Bookings
                .Include(t => t.Tour)
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(t => t.BookingId == model.BookingId);

            if(booking == null)
            {
                TempData["ErrorMessage"] = "Booking không tồn tại.";
                return RedirectToAction("Index", "Home");
            }

            booking.BookingDate = model.BookingDate;
            booking.Status = model.Status;
            booking.TotalPrice = model.Price;
            booking.TourId = model.TourId;
            booking.CustomerId = model.CustomerId;

            await _context.SaveChangesAsync();

            TempData["Message"] = "Sửa booking thành công!";
            return RedirectToAction("Booking", "AdminBooking");
        }
    }
}
