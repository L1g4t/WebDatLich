using Microsoft.AspNetCore.Mvc;

namespace WebDatLich.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
