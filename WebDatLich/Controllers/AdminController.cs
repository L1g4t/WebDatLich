using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebDatLich.Data;
using WebDatLich.Models;

namespace WebDatLich.Controllers
{
	public class AdminController : Controller
	{
		private readonly CsdlDuLichContext _context;

		public AdminController(CsdlDuLichContext context)
		{
			_context = context;
		}

        public async Task<IActionResult> Tours(string searchString, string sortOrder)
        {
            // Tạo query cho các tour
            var toursQuery = _context.Tours
                .Include(t => t.Destination)
                .AsQueryable();

            // Nếu có tìm kiếm, thêm điều kiện tìm kiếm vào query
            if (!string.IsNullOrEmpty(searchString))
            {
                toursQuery = toursQuery.Where(t => t.TourName.Contains(searchString) || t.Description.Contains(searchString));
            }

            // Xử lý sắp xếp theo cột
            switch (sortOrder)
            {
                case "id_asc":
                    toursQuery = toursQuery.OrderBy(t => t.TourId);
                    break;
                case "id_desc":
                    toursQuery = toursQuery.OrderByDescending(t => t.TourId);
                    break;
                case "name_asc":
                    toursQuery = toursQuery.OrderBy(t => t.TourName);
                    break;
                case "name_desc":
                    toursQuery = toursQuery.OrderByDescending(t => t.TourName);
                    break;
                case "price_asc":
                    toursQuery = toursQuery.OrderBy(t => t.Price);
                    break;
                case "price_desc":
                    toursQuery = toursQuery.OrderByDescending(t => t.Price);
                    break;
                default:
                    toursQuery = toursQuery.OrderBy(t => t.TourId); // Sắp xếp mặc định theo ID
                    break;
            }

            // Lấy danh sách các tour đã lọc và sắp xếp
            var tours = await toursQuery.ToListAsync();

            // Truyền các tham số sắp xếp và tìm kiếm vào ViewData để dùng trong View
            ViewData["CurrentFilter"] = searchString;
            ViewData["IdSortParm"] = sortOrder == "id_asc" ? "id_desc" : "id_asc";
            ViewData["NameSortParm"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["PriceSortParm"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";

            return View(tours);
        }

        // Thêm Tour
        [HttpGet]
		public IActionResult AddTour()
		{
            var destinations = _context.Destinations
                .Select(d => new SelectListItem
                {
                    Value = d.DestinationId.ToString(),
                    Text = d.DestinationName
                })
                .ToList();

            if (!destinations.Any())
            {
                TempData["ErrorMessage"] = "Không có địa điểm nào để thêm Tour. Vui lòng thêm địa điểm trước.";
                return RedirectToAction("Destination", "AdminDestination");
            }

            var model = new AddTourViewModel
            {
                Destinations = destinations
            };

            return View(model);
        }

		[HttpPost]
        public async Task<IActionResult> AddTour(AddTourViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Nếu có lỗi, load lại danh sách địa điểm
                model.Destinations = _context.Destinations
                    .Select(d => new SelectListItem
                    {
                        Value = d.DestinationId.ToString(),
                        Text = d.DestinationName
                    })
                    .ToList();

                return View(model);
            }

            var destinationExists = await _context.Destinations
                .AnyAsync(d => d.DestinationId == model.DestinationId);

            if (!destinationExists)
            {
                ModelState.AddModelError("DestinationId", "Địa điểm không hợp lệ.");

                // Load lại danh sách địa điểm
                model.Destinations = _context.Destinations
                    .Select(d => new SelectListItem
                    {
                        Value = d.DestinationId.ToString(),
                        Text = d.DestinationName
                    })
                    .ToList();

                return View(model);
            }

            var tour = new Tour
            {
                TourName = model.TourName,
                Description = model.Description,
                Price = model.Price,
                StartDay = model.StartDay,
                EndDay = model.EndDay,
                DestinationId = model.DestinationId
            };

            // Lưu tour vào cơ sở dữ liệu
            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Thêm Tour thành công!";
            return RedirectToAction("Tours", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTour(int id)
        {
            // Kiểm tra xem Tour có liên kết với Booking không
            var isForeignKey = await _context.Bookings.AnyAsync(b => b.TourId == id);

            if (isForeignKey)
            {
                TempData["ErrorMessage"] = "Không thể xóa Tour vì đang được sử dụng trong Booking.";
                return RedirectToAction("Tours","Admin"); // Chuyển hướng về danh sách
            }

            var tour = await _context.Tours.FindAsync(id);
            if (tour == null)
            {
                TempData["ErrorMessage"] = "Tour không tồn tại.";
                return RedirectToAction("Tours", "Admin");
            }

            _context.Tours.Remove(tour);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Xóa Tour thành công!";
            return RedirectToAction("Tours", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> EditTour(int id)
        {
            var tour = await _context.Tours
                .Include(t => t.Destination)  
                .FirstOrDefaultAsync(t => t.TourId == id);

            if (tour == null)
            {
                TempData["ErrorMessage"] = "Tour không tồn tại.";
                return RedirectToAction("Tours", "Admin");
            }

            var model = new EditTourViewModel
            {
                TourId = tour.TourId,
                TourName = tour.TourName,
                Description = tour.Description,
                Price = tour.Price,
                StartDay = tour.StartDay,
                EndDay = tour.EndDay,
                DestinationId = tour.DestinationId,
                Destinations = await _context.Destinations
                    .Select(d => new SelectListItem
                    {
                        Value = d.DestinationId.ToString(),
                        Text = d.DestinationName
                    })
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTour(EditTourViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Nếu có lỗi, load lại danh sách địa điểm
                model.Destinations = await _context.Destinations
                    .Select(d => new SelectListItem
                    {
                        Value = d.DestinationId.ToString(),
                        Text = d.DestinationName
                    })
                    .ToListAsync();

                return View(model);
            }

            var destinationExists = await _context.Destinations
                .AnyAsync(d => d.DestinationId == model.DestinationId);

            if (!destinationExists)
            {
                ModelState.AddModelError("DestinationId", "Địa điểm không hợp lệ.");

                // Load lại danh sách địa điểm
                model.Destinations = await _context.Destinations
                    .Select(d => new SelectListItem
                    {
                        Value = d.DestinationId.ToString(),
                        Text = d.DestinationName
                    })
                    .ToListAsync();

                return View(model);
            }

            var tour = await _context.Tours
                .Include(t => t.Destination)
                .FirstOrDefaultAsync(t => t.TourId == model.TourId);

            if (tour == null)
            {
                TempData["ErrorMessage"] = "Tour không tồn tại.";
                return RedirectToAction("Tour","Admin");
            }

            // Cập nhật thông tin Tour
            tour.TourName = model.TourName;
            tour.Description = model.Description;
            tour.Price = model.Price;
            tour.StartDay = model.StartDay;
            tour.EndDay = model.EndDay;
            tour.DestinationId = model.DestinationId;

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            TempData["Message"] = "Sửa Tour thành công!";
            return RedirectToAction("Tours", "Admin");
        }


    }
}
