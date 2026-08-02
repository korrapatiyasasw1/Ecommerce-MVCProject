using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.Models;
using MVCDotnetCore.Services;
using Org.BouncyCastle.Utilities;
using System.Security.Claims;

namespace MVCDotnetCore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService OrderService;
        private readonly ICustomerAddressService CustomerAddressService;
        private readonly PDFService pdfservice;
       
        public OrderController(IOrderService _OrderService,
            ICustomerAddressService _CustomerAddressService,
            PDFService pdfservice)
        {
            OrderService = _OrderService;
            CustomerAddressService = _CustomerAddressService;
            this.pdfservice = pdfservice;

        }
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> GenerateInvoicePdf(int orderId)
        {
            int UserId = int.Parse(
User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var order = await OrderService.GetOrderId(UserId, orderId);

            byte[] pdf = pdfservice.GeneratepdfInvoice(order) ;
            return File(pdf, "application/pdf", 
                "Invoice.pdf");
        }

        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> GetOrder()
        {
            int UserId = int.Parse(
 User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var order = await OrderService.GetOrders(UserId);
            return View(order);

        }
        [Authorize(Roles = "Customer,Admin")]

        [HttpGet]
        public async Task<IActionResult> GetOrderId( int orderid)
        {
            int userId = int.Parse(
User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var order = await OrderService.GetOrderId(userId,orderid);
            return View(order);
        }

        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> GetOrderItemByOrderId(int id)
        {
            var orderitem = await OrderService.GetOrderItemByOrderId(id);
            return View(orderitem);
        }

        [Authorize(Roles = "Customer")]

        [HttpGet]
        public async Task<IActionResult> CreateOrderView()
        {
            int UserId = int.Parse(
 User.FindFirst(ClaimTypes.NameIdentifier)!.Value); 
            var Address = await CustomerAddressService.GetCustomerAddressById(UserId);
            ViewBag.CustomerAddress = Address.Select(x => new
            {
                Value = x.Id,
                Display =$"{x.HouseNo} and{x.Street}{x.City} {x.State} {x.PostalCode} {x.Country}"
            });
            return View();
        }
        [Authorize(Roles = "Customer")]

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            int UserId = int.Parse(
 User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
          
            var createorder = await  OrderService.CreateOrder(UserId,order);
            return RedirectToAction("GetOrderId" ,new {orderid=createorder.Id});
        }
        [Authorize(Roles = "Customer")]

        [HttpPost]
        public async Task<IActionResult> GetOrderByDate( DateOnly? orderdate)
        {
            int userId = int.Parse(
User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if(orderdate==null)
            {
                return RedirectToAction("GetOrder");
            }
            var order = await OrderService.GetByDate(userId,orderdate);
            return View("GetOrder", order);
        }
    }
}
