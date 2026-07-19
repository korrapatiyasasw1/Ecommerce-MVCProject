using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVCDotnetCore.Models;
using MVCDotnetCore.Services;
using System.Diagnostics;
using System.Security.Claims;

namespace MVCDotnetCore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService CartService;

        public CartController(ICartService _CartService)
        {
            CartService = _CartService;

        }

        [HttpGet]
        public async Task<IActionResult> IndexCart()
        {
            int UserId = int.Parse(
User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var cart = await CartService.GetCart(UserId);

            return View(cart);
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int UserId = int.Parse(
User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var cartItem = await CartService.GetCartItem(UserId);
            if(cartItem == null)
            {
                return View("Empty","Cart");
            }
            return View(cartItem);
        }
        [HttpPost]
        public async Task<IActionResult> AddCustomerCart(int productId)
        {
            Debug.WriteLine($"Product ID: {productId}");
            int UserId = int.Parse(
User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await CartService.AddCart(UserId,productId);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(int itemId)
        {
            int UserId = int.Parse(
User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
             await CartService.AddQuantity(UserId,itemId);
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> DeceraseQuantity(int itemId)
        {
            int UserId = int.Parse(
User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await CartService.DeceraseQuantity(UserId, itemId);
            return RedirectToAction("Index");
        }




    }
}
