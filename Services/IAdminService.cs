using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.DTOs;
using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public delegate Task OrderHandler(Order order);

    public interface IAdminService
    {
         event OrderHandler? Adminstatus;
        Task Orderstatus(Order order);
        Task<AdminDTO> AdminView();
        Task<List<Order>> GetAllOrder();
        Task<Order> UpdateOrderStatus(int id);
        Task<List<Product>> GetAllProducts();


    }
}
