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

		//Hiện thị thông tin nhân viên
		public IActionResult Show(string username)
		{
			var user = _context.Accounts
				.Include(u => u.Employee)
				.FirstOrDefault(u => u.Username == username);
			if (user == null) return NotFound();

			return View(user);
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
        public IActionResult Edit(string username , string name, string pass,string phone, string adress,string email)
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
                user.Customer.Address = adress;
                user.Customer.PhoneNumber = phone;
                user.Customer.Email = email;
            }

            _context.SaveChanges();

            ViewBag.Message = "Cập nhật thành công!";
            return View(user);
        }
    }
}
