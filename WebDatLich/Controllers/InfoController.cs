using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDatLich.Data;

namespace WebDatLich.Controllers
{
	public class InfoController : Controller
	{
        private readonly CsdlDuLichContext _context;

        public InfoController(CsdlDuLichContext context)
        {
            _context = context;
        }

        //Hiển thị thông tin người dùng 
        public IActionResult Edit(string username)
		{
            var user = _context.Accounts
                .Include(u => u.Customer)
                .FirstOrDefault(u => u.Username == username);
            if (user == null) return NotFound();

            return View(user);
		}

        // Cập nhật thông tin người dùng 
        [HttpPost]
        public IActionResult Edit(string username , string name, string pass)
        {
            var user = _context.Accounts
            .Include(u => u.Customer) // Load dữ liệu từ bảng Profiles
            .FirstOrDefault(u => u.Username == username);

            if (user == null) return NotFound();

            // Cập nhật thông tin
            user.Password = pass;
            if (user.Customer != null)
            {
                user.Customer.FullName = name;
            }

            _context.SaveChanges();

            ViewBag.Message = "Cập nhật thành công!";
            return View(user);
        }
    }
}
