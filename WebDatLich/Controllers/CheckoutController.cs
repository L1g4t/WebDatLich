using Microsoft.AspNetCore.Mvc;

namespace WebDatLich.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
