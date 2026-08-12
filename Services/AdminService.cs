using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCDotnetCore.Data;
using MVCDotnetCore.DTOs;
using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        public event OrderHandler? Adminstatus;
        private readonly PDFService _pdfService;
        public AdminService(AppDbContext context, IEmailService emailService
            ,PDFService pdfService)
        {
            _context = context;
            _emailService = emailService;
            _pdfService = pdfService;
        }
        public async Task<AdminDTO> AdminView()
        {
            var CountView =await  _context.Order
                    .ToListAsync();
            var adminDTO = new AdminDTO();
            adminDTO.TotalOrders = CountView.Count();
            adminDTO.TotalCustomers = CountView.Select(x => x.CustomerId).
                            Distinct().Count() ;
            adminDTO.TotalRevunue = CountView.Select(x => x.TotalPrice).Sum();
            adminDTO.TotalProducts = await _context.Products.CountAsync(); 
            return adminDTO;
        }
        public async Task<List<Order>> GetAllOrder()
        {
            var order = await _context.Order.Include(x=>x.Customer).
                Include(x=>x.CustomerAddress).
                Include(x=>x.OrderItems).
                ToListAsync();
            if(order == null)
            {
                return null;
            }
            return order;
        }
        public async Task<Order> UpdateOrderStatus(int id)
        {
            var order = await _context.Order.
                FirstOrDefaultAsync(x => x.Id == id);
            return order;
        }
        public async Task Orderstatus(Order order)
        {
            var orderstatus = await _context.Order.Include(x=>x.OrderItems).
                ThenInclude(x=>x.Product).
                Include(x=>x.Customer).
                ThenInclude(x=>x.User).
                Include(x => x.CustomerAddress).
                FirstOrDefaultAsync(x => x.Id == order.Id);

            if (order.OrderStatus == "Shipped")
            {
                await _emailService.SendOrderStatusEmail(
                           orderstatus.Customer.Email,
                           orderstatus.Customer.LastName,
                       orderstatus.OrderNumber,
                       orderstatus.OrderStatus);
            }
            if(order.OrderStatus == "Delivered")
            {
                orderstatus.OrderStatus = "Delivered";
                await _context.SaveChangesAsync();
                byte[] sendpdf = _pdfService.GeneratepdfInvoice(orderstatus);
               await  _emailService.SendOrderInvoiceMail(orderstatus, sendpdf);
            }
        }
        public async Task<List<Product>> GetAllProducts()
        {
            var product = await _context.Products.ToListAsync();
            return product;
        }
    }
}
