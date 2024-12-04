using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebDatLich.Data;
using WebDatLich.Models;

namespace WebDatLich.Controllers
{
    public class AdminEmployeeController : Controller
    {
        private readonly CsdlDuLichContext _context;

        public AdminEmployeeController(CsdlDuLichContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Employee(string searchString, string sortOrder)
        {

            var employeeQuery = _context.Employees 
                .Include(t => t.TourGuides)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                employeeQuery = employeeQuery.Where(t => t.FullName.Contains(searchString));
            }

            var employee = await employeeQuery.ToListAsync();
            return View(employee);
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            var model = new AdminEmployeeViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(AdminEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var employee = new Employee
            {
                FullName=model.FullName,
                Position=model.Position,
                HireDate=model.HireDate,
                PhoneNumber=model.PhoneNumber
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Nếu chức vụ là Tour Guide, chuyển hướng đến form nhập thông tin Tour Guide
            if (model.Position == "Tour Guide")
            {
                // Lấy EmployeeId vừa được tạo
                var newEmployeeId = employee.EmployeeId;
                return RedirectToAction("AddTourGuide", new { employeeId = newEmployeeId });
            }

            TempData["Message"] = "Thêm nhân viên thành công!";
            return RedirectToAction("Employee", "AdminEmployee");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var isForeignKey = await _context.Accounts.AnyAsync(b => b.EmployeeId == id);

            if (isForeignKey)
            {
                TempData["ErrorMessage"] = "Hãy xóa Tài khoản của nhân viên này trước";
                return RedirectToAction("Employee", "AdminEmployee");
            }

            var employee = await _context.Employees
                .Include(t=>t.TourGuides)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (employee == null)
            {
                TempData["ErrorMessage"] = "Nhân viên không tồn tại.";
                return RedirectToAction("Employee", "AdminEmployee");
            }

            // Nếu nhân viên có liên kết với TourGuide, xóa TourGuide
            if (employee.TourGuides != null)
            {
                _context.TourGuides.Remove(employee.TourGuides);
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Xóa nhân viên thành công!";
            return RedirectToAction("Employee", "AdminEmployee");
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(int id)
        {
            var employee = await _context.Employees
                .Include(t => t.TourGuides)
                .FirstOrDefaultAsync(t => t.EmployeeId == id);

            if (employee == null)
            {
                TempData["ErrorMessage"] = "Nhân viên không tồn tại.";
                return RedirectToAction("Employee", "AdminEmployee");
            }

            var model = new AdminEmployeeViewModel
            {
                EmployeeId= employee.EmployeeId,
                FullName=employee.FullName,
                Position=employee.Position,
                HireDate=employee.HireDate,
                PhoneNumber=employee.PhoneNumber,
                // Nếu là Tour Guide, có thể thêm thông tin TourGuide
                TourGuideExperience = employee.TourGuides?.ExperienceYears,
                TourGuideLanguages = employee.TourGuides?.LanguageSpoken
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(AdminEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var employee = await _context.Employees
                .Include(t=>t.TourGuides)
                .FirstOrDefaultAsync(t => t.EmployeeId == model.EmployeeId);

            if (employee == null)
            {
                TempData["ErrorMessage"] = "Nhân viên không tồn tại.";
                return RedirectToAction("Employee", "AdminEmployee");
            }

            employee.EmployeeId = model.EmployeeId;
            employee.FullName = model.FullName;
            employee.Position = model.Position;
            employee.HireDate = model.HireDate;
            employee.PhoneNumber = model.PhoneNumber;
            // Nếu nhân viên là Tour Guide, cập nhật thêm thông tin TourGuide
            if (employee.Position == "Tour Guide")
            {
                if (employee.TourGuides == null)
                {
                    // Nếu chưa có TourGuide, tạo mới
                    employee.TourGuides = new TourGuide
                    {
                        EmployeeId = employee.EmployeeId,
                        ExperienceYears = model.TourGuideExperience,
                        LanguageSpoken = model.TourGuideLanguages
                    };
                }
                else
                {
                    // Nếu đã có TourGuide, cập nhật thông tin
                    employee.TourGuides.ExperienceYears = model.TourGuideExperience;
                    employee.TourGuides.LanguageSpoken = model.TourGuideLanguages;
                }
            }
            else
            {
                // Nếu không phải Tour Guide, xóa TourGuide nếu có
                if (employee.TourGuides != null)
                {
                    _context.TourGuides.Remove(employee.TourGuides);
                    employee.TourGuides = null;
                }
            }
            await _context.SaveChangesAsync();

            TempData["Message"] = "Sửa thông tin nhân viên thành công!";
            return RedirectToAction("Employee", "AdminEmployee");
        }

        [HttpGet]
        public IActionResult AddTourGuide(int employeeId)
        {
            var model = new AdminTourGuideViewModel
            {
                EmployeeId = employeeId // Truyền EmployeeId sang form
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddTourGuide(AdminTourGuideViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Thêm thông tin vào bảng TourGuides
            var tourGuide = new TourGuide
            {
                EmployeeId = model.EmployeeId,
                ExperienceYears = model.ExperienceYears,
                LanguageSpoken = model.LanguageSpoken
            };

            _context.TourGuides.Add(tourGuide);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Thêm hướng dẫn viên thành công!";
            return RedirectToAction("Employee", "AdminEmployee");
        }
    }
}
