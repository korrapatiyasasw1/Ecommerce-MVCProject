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
        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

           var customer=  await CustomerService.GetCustomerById(userId);
            if (customer == null)
            {
                return Content("Customer not found");
            }


            return View(customer);
        }
        public IActionResult AddProfile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProfile(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    Console.WriteLine(error.Key);

                    foreach (var e in error.Value.Errors)
                    {
                        Console.WriteLine(e.ErrorMessage);
                    }
                }
            }
            int userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if(ModelState.IsValid)
            {
               await CustomerService.AddCustomer(customer,userId);
                return RedirectToAction("Index");
            }
            return View();
        }
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
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(Customer customer)
        {
            int userId = int.Parse(
           User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
           if(ModelState.IsValid)
            {
                await CustomerService.UpdateCustomer(customer, userId);
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
