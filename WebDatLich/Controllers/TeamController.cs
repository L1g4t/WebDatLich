using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatLich.Data;

namespace WebDatLich.Controllers
{
    public class TeamController : Controller
    {
        private readonly CsdlDuLichContext  _context;

        public TeamController(CsdlDuLichContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tourGuides = _context.TourGuides.Include(tg => tg.Employee).ToList();
            return View(tourGuides);
        }
    }
}
