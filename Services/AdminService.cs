using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCDotnetCore.Data;
using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        public AdminService(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
            var orderstatus = await _context.Order.
                Include(x=>x.Customer).ThenInclude(x=>x.User).
                FirstOrDefaultAsync(x => x.Id == order.Id);
            orderstatus.OrderStatus = order.OrderStatus;
            await _context.SaveChangesAsync();
            if (orderstatus.OrderStatus == "Shipped")
            {
                await _emailService.SendOrderStatusEmail(
                           orderstatus.Customer.User.Email,
                           orderstatus.Customer.LastName,
                       orderstatus.OrderNumber,
                       orderstatus.OrderStatus);
            }

        }
        public async Task<List<Product>> GetAllProducts()
        {
            var product = await _context.Products.ToListAsync();
            return product;
        }
    }
}
