using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
      //  Task GetProductById(int id);
        Task AddProduct(Product product);
        Task<Product> GetProductById(int id);
        Task UpdateProduct(Product product);
    //    Task<Product> DeleteProduct(int id);
        Task DeleteProduct(int id);
        Task<List<Product>> Search(string ProductName);
        Task<List<Product>> SearchByCategoryName(string CategoryName);
      //  Task<List<Product>> OrderByPrice(int MinPrice,int MaxPrice);

    }
}
