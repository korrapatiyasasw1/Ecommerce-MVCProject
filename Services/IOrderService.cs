using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public interface IOrderService
    {
        Task CreateOrder(int userId, Order order);

       // Task<List<Order>> GetOrders(int userId);

        // Task<Order> GetOrderById(int orderId);

        //Task CancelOrder(int orderId);
    }
}
