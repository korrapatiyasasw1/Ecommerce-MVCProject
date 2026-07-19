using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.Models;
using System.Diagnostics;

namespace MVCDotnetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
