using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebDatLich.Data;
using WebDatLich.Models;

namespace WebDatLich.Controllers
{
    public class AdminDestinationController : Controller
    {
        private readonly CsdlDuLichContext _context;

        public AdminDestinationController(CsdlDuLichContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Destination(string searchString, string sortOrder)
        {

            var destinationQuery = _context.Destinations
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                destinationQuery = destinationQuery.Where(t => t.DestinationName.Contains(searchString)
                || t.Description.Contains(searchString));
            }

            var destination = await destinationQuery.ToListAsync();
            return View(destination);
        }

        [HttpGet]
        public IActionResult AddDestination()
        {
            var model = new AdminDestinationViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddDestination(AdminDestinationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var destination = new Destination
            {
                DestinationName = model.DestinationName,
                Description = model.Description
            };

            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Thêm Địa điểm thành công!";
            return RedirectToAction("Destination", "AdminDestination");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDestination(int id)
        {

            var isForeignKey = await _context.Tours.AnyAsync(b => b.DestinationId == id);

            if (isForeignKey)
            {
                TempData["ErrorMessage"] = "Không thể xóa vì có đang có tour đến địa điểm này";
                return RedirectToAction("Destination", "AdminDestination"); 
            }

            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null)
            {
                TempData["ErrorMessage"] = "Địa điểm không tồn tại.";
                return RedirectToAction("Destination", "AdminDestination");
            }

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Xóa địa điểm thành công!";
            return RedirectToAction("Destination", "AdminDestination");
        }
        
        [HttpGet]
        public async Task<IActionResult> EditDestination(int id)
        {
            var destination = await _context.Destinations
                .FirstOrDefaultAsync(t => t.DestinationId == id);

            if (destination == null)
            {
                TempData["ErrorMessage"] = "Địa điểm không tồn tại.";
                return RedirectToAction("Destination", "AdminDestination");
            }

            var model = new AdminDestinationViewModel
            {
                DestinationId = destination.DestinationId,
                DestinationName = destination.DestinationName,
                Description=destination.Description
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDestination(AdminDestinationViewModel model)
        {
            if (!ModelState.IsValid)
            {               
                return View(model);
            }

            var destination = await _context.Destinations 
                .FirstOrDefaultAsync(t => t.DestinationId == model.DestinationId);

            if (destination == null)
            {
                TempData["ErrorMessage"] = "Địa điểm không tồn tại.";
                return RedirectToAction("Destination", "AdminDestination");
            }

            destination.Description = model.Description;
            destination.DestinationName = model.DestinationName;
            destination.DestinationId = model.DestinationId;

            await _context.SaveChangesAsync();

            TempData["Message"] = "Sửa địa điểm thành công!";
            return RedirectToAction("Destination", "AdminDestination");
        }
    }
}
