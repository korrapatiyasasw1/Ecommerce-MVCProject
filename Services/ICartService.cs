using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.Models;

namespace MVCDotnetCore.Services
{
    public interface ICartService
    {
        Task AddCart(int UserId,int productId);
        Task<Cart> GetCart(int UserId);
        Task<ViewModel> GetCartItem(int UserId);
        Task<CartItem> AddQuantity(int UserId, int itemId);
       Task<CartItem> DeceraseQuantity(int UserId,  int itemId);

    }
}
