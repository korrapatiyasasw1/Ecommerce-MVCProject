using Microsoft.EntityFrameworkCore;
using MVCDotnetCore.Data;
using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly PDFService pdfservice;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetOrders(int userId)
        {
            var customer = await _context.Customers.
    FirstOrDefaultAsync(x => x.UserId == userId);
            if (customer == null) {
                return null;
            }

            var orderitems = await _context.Order.Include(x => x.OrderItems).
                ThenInclude(x=>x.Product).
                Where(x => x.CustomerId == customer.Id).ToListAsync();

     
            if(orderitems == null)
            {
                return null;
            }
            return orderitems;
        }
        public  async  Task<Order> GetOrderId(int userId,int orderid)
        {
            var customer = await _context.Customers.
                           FirstOrDefaultAsync(x => x.UserId == userId);
            if (customer == null)
            {
                return null;
            }
            var order =await  _context.Order.Include(x=>x.OrderItems).ThenInclude(x=>x.Product).
                FirstOrDefaultAsync(x=>x.CustomerId == customer.Id
            && x.Id == orderid);
            if (order == null)
            {
                return null;
            }
            return order;
        }
        public async Task<List<OrderItem>> GetOrderItemByOrderId(int id)
        {
            var orderitem  = await _context.OrderItems.
                Include(x=>x.Order).Include(x=>x.Product).
                Where(x=>x.OrderId==id).ToListAsync();
            if (orderitem == null)
            {
                return null;
            }
            return orderitem;
        }

        public async Task<Order> CreateOrder(int userId,Order order)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var customer = await _context.Customers.
                FirstOrDefaultAsync(x => x.UserId == userId);
                if (customer == null)
                {
                    throw new Exception("Customer not found " +
                        "for this order");
                }
                var customeraddress = await _context.CustomerAddresses.
                    Where(x => x.CustomerId == customer.Id).ToListAsync();
                if (!customeraddress.Any())
                {
                    throw new Exception("Customer address" +
                        " not found for this order");
                }
                var postorder = new Order()
                {
                    CustomerId = customer.Id,
                    CustomerAddressId = order.CustomerAddressId,
                    OrderDate = DateTime.Now,
                    OrderNumber = Guid.NewGuid().ToString(),
                    OrderStatus = "pending",
                    TotalPrice = 0
                };

                _context.Order.Add(postorder);
                await _context.SaveChangesAsync();
                var cartitem = await _context.CartItems.
                    Where(x => x.Cart.CustomerId == 
                    customer.Id).ToListAsync();
                if (!cartitem.Any())
                {
                    throw new Exception("cart item is null for this customerid");
                }
                int totalprice = 0;
                foreach (var item in cartitem)
                {
                    var orderitem = new OrderItem();

                    orderitem.OrderId = postorder.Id;
                    orderitem.UnitPrice = item.UnitPrice;
                    orderitem.Quantity = item.Quantity;
                    orderitem.productid = item.ProductId;
                    _context.OrderItems.Add(orderitem);
                    totalprice = totalprice + ((item.Quantity) *
                        (int)item.UnitPrice);
                    _context.CartItems.Remove(item);
                    var product = _context.Products.
                        FirstOrDefault(x => x.Id == item.ProductId);
                    if(product.stock == 0)
                    {
                        throw new Exception("stock not there");
                    }
                    product.stock = product.stock - item.Quantity;

                }
                
                postorder.TotalPrice = totalprice;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return postorder;
            }


            catch
            {
                await transaction.RollbackAsync();
                throw;
            }




        }
        public async Task<List<Order>> GetByDate(int userId, DateOnly? orderdate)
        {
            if (orderdate == null)
            {
                return null;
            }

            DateTime dateTime = orderdate.Value.ToDateTime(TimeOnly.MinValue);


            var customer = await _context.Customers.
                FirstOrDefaultAsync(x => x.UserId == userId);
            if (customer == null) 
            {
                return null;
            }
            var order = await _context.Order.Include(x=>x.OrderItems).ThenInclude(x=>x.Product).
                Where(x=>x.OrderDate.Date == dateTime.Date
                        && x.CustomerId == customer.Id
                ).ToListAsync();
            if(order == null)
            {
                return null;
            }
            return order;
        }
       public async  Task<Order> GetOrderIdWithoutUser(int orderId)
        {
            var order = await _context.Order.Include(c=>c.Customer).Include(c => c.CustomerAddress).
                Include(x => x.OrderItems).ThenInclude(x => x.Product).
                FirstOrDefaultAsync(x => x.Id == orderId);
            return order;
        }


    }
}
