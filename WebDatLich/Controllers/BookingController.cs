using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatLich.Data;


namespace WebDatLich.Controllers
{
    public class BookingController : Controller
    {
        private readonly CsdlDuLichContext _context;

        public BookingController(CsdlDuLichContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var userid = HttpContext.Session.GetString("Username");
            var bookings = _context.Bookings
                .Include(b => b.Tour)
                    .ThenInclude(c => c.Destination)
                .Include(d => d.Customer)
                    .ThenInclude(e => e.Accounts)
                .Where(b => b.Customer.Accounts.Any(a => a.Username == userid))
                .ToList();
            return View(bookings);
        }
    }
}
