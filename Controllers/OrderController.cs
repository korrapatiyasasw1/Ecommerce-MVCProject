using Microsoft.AspNetCore.Mvc;

namespace MVCDotnetCore.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
