using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public interface IAdminService
    {
        Task<AdminDTO> AdminView();
        Task<List<Order>> GetAllOrder();
        Task<Order> UpdateOrderStatus(int id);
        Task Orderstatus(Order order);
        Task<List<Product>> GetAllProducts();


    }
}
