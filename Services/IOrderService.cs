using MailKit.Search;
using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public delegate Task OrderCreatedHandler(Order order);
    public interface IOrderService
    {
        event OrderCreatedHandler? OrderCreated;
        Task<Order> CreateOrder(int userId, Order order);

       Task<List<Order>> GetOrders(int userId);

        Task<Order> GetOrderId(int userId,int orderid);

        Task<List<Order>> GetByDate(int userId,DateOnly? orderdate);
        Task<Order> GetOrderIdWithoutUser(int orderId);
            
        //Task CancelOrder(int orderId);
        Task<List<OrderItem>> GetOrderItemByOrderId(int id);
    }
}
