using Microsoft.EntityFrameworkCore;
using MVCDotnetCore.Data;
using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrder(int userId,Order order)
        {
            var customer = await _context.Customers.
                FirstOrDefaultAsync(x=>x.UserId == userId);
            if(customer == null)
            {
                throw new Exception("Customer not found for this order");
            }
            var customeraddress = await  _context.CustomerAddresses.
                Where(x => x.CustomerId == customer.Id).ToListAsync();
            if(!customeraddress.Any())
            {
                throw new Exception("Customer address not found for this order");
            }
            var postorder = new Order() {

                CustomerId = customer.Id,
                CustomerAddressId = order.CustomerAddressId,
                OrderDate = DateTime.Now,
                OrderNumber = "0",
                TotalPrice = 0
            };
            
            _context.Order.Add(postorder);
            await _context.SaveChangesAsync();

            var cartitem = await _context.CartItems.
                Where(x => x.Cart.CustomerId == customer.Id).ToListAsync();
            if(!cartitem.Any())
            {
                throw new Exception("cart item is null for this customerid");
            }
            int totalprice = 0;
            foreach(var item in cartitem)
            {
                var orderitem = new OrderItem();

                orderitem.OrderId = postorder.Id;
                orderitem.UnitPrice = item.UnitPrice;
                orderitem.Quantity = item.Quantity;
                orderitem.productid = item.ProductId;
                _context.OrderItems.Add(orderitem);
                totalprice = totalprice + ((item.Quantity) *(int) item.UnitPrice);
                _context.CartItems.Remove(item);

            }
            postorder.TotalPrice = totalprice;
            await _context.SaveChangesAsync();
            
        }
    }
}
