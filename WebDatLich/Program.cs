using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebDatLich.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CsdlDuLichContext>(options => 
{ options.UseSqlServer(builder.Configuration.GetConnectionString("CSDL_DuLich")); });

// Cấu hình session và cache
builder.Services.AddDistributedMemoryCache(); // Sử dụng bộ nhớ phân tán để lưu trữ session
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn của session
	options.Cookie.HttpOnly = true; // Bảo vệ cookie khỏi việc truy cập JavaScript
	options.Cookie.IsEssential = true; // Đảm bảo cookie là thiết yếu
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/Account/Login"; // Trang đăng nhập
		options.LogoutPath = "/Account/Logout"; // Trang đăng xuất
        options.ExpireTimeSpan = TimeSpan.FromDays(1); // Thời gian hết hạn mặc định của cookie
        options.SlidingExpiration = true; // Cookie sẽ tự động gia hạn khi người dùng truy cập
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
