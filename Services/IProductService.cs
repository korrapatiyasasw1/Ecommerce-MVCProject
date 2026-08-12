using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.Models;
using MVCDotnetCore.DTOs;   

namespace MVCDotnetCore.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProducts();
      //  Task GetProductById(int id);
        Task AddProduct(Product product);
        Task<Product> GetProductByIdForEdit(int id);
        Task<ProductDetailsDTo> GetProductById(int id);
        Task UpdateProduct(Product product);
    //    Task<Product> DeleteProduct(int id);
        Task DeleteProduct(int id);
        Task<List<ProductDto>> Search(string ProductName);
        Task<List<ProductDto>> SearchByCategoryName(string CategoryName);
      //  Task<List<Product>> OrderByPrice(int MinPrice,int MaxPrice);

    }
}
