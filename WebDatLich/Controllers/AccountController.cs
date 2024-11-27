using Microsoft.AspNetCore.Mvc;
using WebDatLich.Models;
using Microsoft.AspNetCore.Identity;
using WebDatLich.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace WebDatLich.Controllers
{
    public class AccountController : Controller
    {
		private readonly CsdlDuLichContext _context;

		public AccountController(CsdlDuLichContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				// Kiểm tra thông tin đăng nhập
				var user = await _context.Accounts
					.FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);

				if (user != null)
				{
					// Lưu thông tin vào Session
					HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role);

                    //thông tin người dùng
                    var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, user.Username),
						new Claim(ClaimTypes.Role, user.Role)
					};

					// Tạo Identity và Principal
					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var principal = new ClaimsPrincipal(identity);

					// Tạo thông tin đăng nhập (cookie)
					var authProperties = new AuthenticationProperties
					{
						IsPersistent = model.RememberMe,  // Nếu Remember Me được chọn, cookie sẽ không hết hạn ngay lập tức
						ExpiresUtc = model.RememberMe ? DateTime.UtcNow.AddDays(30) : (DateTime?)null // Cookie nhớ trong 30 ngày
					};

					// Đăng nhập người dùng
					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

					return RedirectToAction("Index", "Home"); 
				}

				ModelState.AddModelError(string.Empty, "Username hoặc mật khẩu không đúng.");
			}

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			// Xóa thông tin trong Session
			HttpContext.Session.Remove("Username");
			// Đăng xuất khỏi cookie authentication
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "Account");
		}


		public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
      	public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				// Kiểm tra nếu người dùng đã tồn tại
				var existingUser = await _context.Accounts.FirstOrDefaultAsync(u => u.Username == model.Username);
				if (existingUser != null)
				{
					ModelState.AddModelError("Username", "Username đã tồn tại.");
					return View(model);
				}

				// Tạo một dòng mới trong bảng Customer
				var newCustomer = new Customer
				{
					FullName = model.Username
				};

				_context.Customers.Add(newCustomer);
				await _context.SaveChangesAsync();

				// Lấy CustomerId cuối cùng từ bảng Customer
				var lastCustomerId = await _context.Customers
					.OrderByDescending(c => c.CustomerId)
					.FirstOrDefaultAsync();

				int CustomerId = lastCustomerId != null ? lastCustomerId.CustomerId : 1;

				// Tạo đối tượng Account mới
				Account account = new Account
				{
					Username = model.Username,
					Password = model.Password,
					Role = "Customer",
					EmployeeId = null,
					CustomerId = CustomerId,
				};
				

				// Thêm vào cơ sở dữ liệu
				_context.Accounts.Add(account);
				await _context.SaveChangesAsync();

				return RedirectToAction("Login");
			}

			return View(model);
		}
	}
}
