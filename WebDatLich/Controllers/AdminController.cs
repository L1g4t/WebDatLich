using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatLich.Data;

namespace WebDatLich.Controllers
{
	[Authorize(Roles = "Admin")] // Đảm bảo chỉ Admin có quyền truy cập
	public class AdminController : Controller
	{
		private readonly CsdlDuLichContext _context;

		public AdminController(CsdlDuLichContext context)
		{
			_context = context;
		}

		// Danh sách các Tour
		public IActionResult Tours()
		{
			var tours = _context.Tours
				.Include(u => u.Destination)
				.ToList(); // Load danh sách Tour cùng các Booking liên quan
			return View(tours);
		}
		/*
		// Thêm Tour
		[HttpGet]
		public IActionResult AddTour()
		{
			return View();
		}

		[HttpPost]
		public IActionResult AddTour(Tour tour)
		{
			if (ModelState.IsValid)
			{
				_context.Tours.Add(tour);
				_context.SaveChanges();
				TempData["Message"] = "Thêm Tour thành công!";
				return RedirectToAction("Tours");
			}
			return View(tour);
		}

		// Sửa Tour
		[HttpGet]
		public IActionResult EditTour(int id)
		{
			var tour = _context.Tours.Find(id);
			if (tour == null) return NotFound();
			return View(tour);
		}

		[HttpPost]
		public IActionResult EditTour(Tour tour)
		{
			if (ModelState.IsValid)
			{
				_context.Tours.Update(tour);
				_context.SaveChanges();
				TempData["Message"] = "Cập nhật Tour thành công!";
				return RedirectToAction("Tours");
			}
			return View(tour);
		}

		// Xóa Tour
		[HttpPost]
		public IActionResult DeleteTour(int id)
		{
			var tour = _context.Tours.Find(id);
			if (tour == null) return NotFound();

			_context.Tours.Remove(tour);
			_context.SaveChanges();
			TempData["Message"] = "Xóa Tour thành công!";
			return RedirectToAction("Tours");
		}

		// Danh sách nhân viên
		public IActionResult Employees()
		{
			var employees = _context.Employees.ToList(); // Lấy danh sách nhân viên
			return View(employees);
		}

		// Danh sách khách hàng
		public IActionResult Customers()
		{
			var customers = _context.Customers.ToList(); // Lấy danh sách khách hàng
			return View(customers);
		}

		// Danh sách Feedbacks
		public IActionResult Feedbacks()
		{
			var feedbacks = _context.Feedbacks.Include(f => f.Customer).Include(f => f.Tour).ToList();
			return View(feedbacks);
		}*/
	}
}
