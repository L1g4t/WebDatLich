using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatLich.Data;

namespace WebDatLich.Controllers
{
    public class TourController : Controller
    {
        private readonly CsdlDuLichContext _context;

        public TourController(CsdlDuLichContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var tour = _context.Tours.Include(tg => tg.Destination).ToList();
            return View(tour);
        }
    }
}
