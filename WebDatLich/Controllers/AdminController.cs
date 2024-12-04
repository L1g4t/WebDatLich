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
                .Include(t =>t.Guide.Employee)
                .AsQueryable();

            // Nếu có tìm kiếm, thêm điều kiện tìm kiếm vào query
            if (!string.IsNullOrEmpty(searchString))
            {
                toursQuery = toursQuery.Where(t => t.TourName.Contains(searchString) || t.Guide.Employee.FullName.Contains(searchString));
            }

            var tours = await toursQuery.ToListAsync();
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
                return RedirectToAction("Tours", "Admin");
            }

            var guide = _context.TourGuides
                .Select(d => new SelectListItem
                {
                    Value = d.GuideId.ToString(),
                    Text = d.Employee.FullName
                })
                .ToList();

            if (!guide.Any())
            {
                TempData["ErrorMessage"] = "Không có tour guide để thêm Tour.";
                return RedirectToAction("Tours", "Admin");
            }

            var model = new AdminTourViewModel
            {
                Destinations = destinations,
                TourGuide = guide
            };

            return View(model);
        }

		[HttpPost]
        public async Task<IActionResult> AddTour(AdminTourViewModel model)
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
                model.TourGuide = _context.TourGuides
                    .Select(d => new SelectListItem
                    {
                        Value = d.GuideId.ToString(),
                        Text = d.Employee.FullName
                    })
                    .ToList();

                return View(model);
            }

            var destinationExists = await _context.Destinations
                .AnyAsync(d => d.DestinationId == model.DestinationId);
            var tourguideExists = await _context.TourGuides
                .AnyAsync(d => d.GuideId == model.GuideId);
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
            if (!tourguideExists)
            {
                ModelState.AddModelError("GuideId", "Tour guide không hợp lệ.");

                // Load lại danh sách địa điểm
                model.TourGuide = _context.TourGuides
                    .Select(d => new SelectListItem
                    {
                        Value = d.GuideId.ToString(),
                        Text = d.Employee.FullName
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
                DestinationId = model.DestinationId,
                GuideId= model.GuideId
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
            var isForeignKey2 = await _context.Feedbacks.AnyAsync(b => b.TourId == id);

            if (isForeignKey)
            {
                TempData["ErrorMessage"] = "Không thể xóa vì tour đã được đặt.";
                return RedirectToAction("Tours","Admin"); // Chuyển hướng về danh sách
            }
            if (isForeignKey2)
            {
                TempData["ErrorMessage"] = "Không thể xóa vì tour đang được đánh giá.";
                return RedirectToAction("Tours", "Admin");
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
                .Include(t=>t.Guide)
                .FirstOrDefaultAsync(t => t.TourId == id);

            if (tour == null)
            {
                TempData["ErrorMessage"] = "Tour không tồn tại.";
                return RedirectToAction("Tours", "Admin");
            }

            var model = new AdminTourViewModel
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
                    .ToListAsync(),
                GuideId = tour.GuideId,
                TourGuide = await _context.TourGuides
                    .Select(d => new SelectListItem
                    {
                        Value = d.GuideId.ToString(),
                        Text = d.Employee.FullName
                    })
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTour(AdminTourViewModel model)
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
                model.TourGuide = _context.TourGuides
                    .Select(d => new SelectListItem
                    {
                        Value = d.GuideId.ToString(),
                        Text = d.Employee.FullName
                    })
                    .ToList();

                return View(model);
            }

            var destinationExists = await _context.Destinations
                .AnyAsync(d => d.DestinationId == model.DestinationId);
            var tourguideExists = await _context.TourGuides
                            .AnyAsync(d => d.GuideId == model.GuideId);
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
            if (!tourguideExists)
            {
                ModelState.AddModelError("GuideId", "Tour guide không hợp lệ.");

                // Load lại danh sách địa điểm
                model.TourGuide = _context.TourGuides
                    .Select(d => new SelectListItem
                    {
                        Value = d.GuideId.ToString(),
                        Text = d.Employee.FullName
                    })
                    .ToList();

                return View(model);
            }

            var tour = await _context.Tours
                .Include(t => t.Destination)
                .Include(t=>t.Guide)
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
            tour.TourId= model.TourId;
            tour.GuideId = model.GuideId;

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            TempData["Message"] = "Sửa Tour thành công!";
            return RedirectToAction("Tours", "Admin");
        }


    }
}
