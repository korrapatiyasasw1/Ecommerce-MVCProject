using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.Models;
using MVCDotnetCore.Services;
using System.Security.Claims;

namespace MVCDotnetCore.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService CustomerService;

        public CustomerController(ICustomerService _CustomerService)
        {
            CustomerService = _CustomerService;
        }
        [Authorize(Roles = "Customer")]

        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
           
                var customer = await CustomerService.GetCustomerById(userId);
            if(customer == null)
            {
                return RedirectToAction("AddProfile");
            }

                return View(customer);
            
           

        }
        [Authorize(Roles = "Customer")]

        public IActionResult AddProfile()
        {
            return View();
        }
        [Authorize(Roles = "Customer")]

        [HttpPost]
        public async Task<IActionResult> AddProfile(Customer customer)
        {
            
            int userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            try
            {
                if (ModelState.IsValid)
                {
                    await CustomerService.AddCustomer(customer, userId);
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
        [Authorize(Roles = "Customer")]

        public async Task<IActionResult> UpdateProfile()
        {
            int userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var customer = await  CustomerService.GetCustomerById(userId);
            if (customer == null)
            {
                return Content("Customer not found");
            }
            return  View(customer);
        }
        [Authorize(Roles = "Customer")]

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(Customer customer)
        {
            int userId = int.Parse(
           User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            try
            {
                if (ModelState.IsValid)
                {
                    await CustomerService.UpdateCustomer(customer, userId);
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

    }
}
