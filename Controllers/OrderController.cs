using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.Models;
using MVCDotnetCore.Services;
using System.Security.Claims;

namespace MVCDotnetCore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService OrderService;
        private readonly ICustomerAddressService CustomerAddressService;
        public OrderController(IOrderService _OrderService,
            ICustomerAddressService _CustomerAddressService)
        {
            OrderService = _OrderService;
            CustomerAddressService = _CustomerAddressService;

        }
        [HttpGet]
        public async Task<IActionResult> CreateOrderView()
        {
            int UserId = int.Parse(
 User.FindFirst(ClaimTypes.NameIdentifier)!.Value); 
            var Address = await CustomerAddressService.GetCustomerAddressById(UserId);
            ViewBag.CustomerAddress = Address.Select(x => new
            {
                Id = x.Id,
                Display = $"{x.HouseNo}, {x.Street}, {x.City}, {x.State}, {x.PostalCode}, {x.Country}"
            });
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            int UserId = int.Parse(
 User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
          
            await  OrderService.CreateOrder(UserId,order);
            return View("CreateOrderView");
        }
    }
}
