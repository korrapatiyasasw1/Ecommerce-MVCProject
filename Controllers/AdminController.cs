using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.Models;
using MVCDotnetCore.Services;

namespace MVCDotnetCore.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService AdminService;
        private readonly IOrderService OrderService;
        private readonly PDFService pdfService;
        private readonly IEmailService _emailService;

        public AdminController(IAdminService _AdminService,
            IOrderService orderService,
            PDFService pdfService,IEmailService emailService)
        {
            AdminService = _AdminService;
            OrderService = orderService;
            this.pdfService = pdfService;
            _emailService = emailService;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DashboardView()
        {
            var Dashboard= await AdminService.AdminView();
            return View(Dashboard);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await AdminService.GetAllOrder();
            if(orders == null)
            {
                return NotFound();
            }
            return View(orders);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateOrderStatus(int id)
        {
            var orders = await AdminService.UpdateOrderStatus( id);
            return View(orders);
        }
        [HttpPost]
        public async Task<IActionResult> Orderstatus(Order order)
        {
            await AdminService.Orderstatus(order);
            var orders = await AdminService.GetAllOrder();
            return View("GetAllOrders",orders);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var product = await AdminService.GetAllProducts();
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> SendPDFToUser(int orderId)
        {
            var orders = await OrderService.GetOrderIdWithoutUser(orderId);
            byte[] pdfBytes =  pdfService.GeneratepdfInvoice(orders);
            var mail = _emailService.SendOrderInvoiceMail(orders, pdfBytes);
            return RedirectToAction("GetAllOrders", "Admin");
        }

    }
}
