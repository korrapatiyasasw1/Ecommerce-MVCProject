using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.Models;
using MVCDotnetCore.Services;
using System.Security.Claims;

namespace MVCDotnetCore.Controllers
{
    public class CustomerAddressController : Controller
    {
        private readonly ICustomerAddressService CustomerAddressService;

        public CustomerAddressController(ICustomerAddressService _CustomerAddressService)
        {
            CustomerAddressService = _CustomerAddressService;
        }
        public IActionResult Index()
        {
            int userId = int.Parse(
        User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetCustomerAddressById()
        {
            int UserId = int.Parse(
        User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if(ModelState.IsValid)
            {
                await CustomerAddressService.GetCustomerAddressById(UserId);
                return RedirectToAction("Index");
            }
            return View();

        }

        [HttpGet]
        public IActionResult AddCustomerAddress()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCustomerAddress(CustomerAddress customerAddress)
        {
            int UserId = int.Parse(
        User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (ModelState.IsValid)
            {
                 await CustomerAddressService.AddCustomerAddress(customerAddress,UserId);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
