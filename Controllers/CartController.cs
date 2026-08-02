using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCDotnetCore.Migrations;
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
        [Authorize(Roles = "Customer")]


        [HttpGet]
        public async Task<IActionResult> IndexCart()
        {
            int UserId = int.Parse(
User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var cart = await CartService.GetCart(UserId);

            return View(cart);
        }
        [Authorize(Roles = "Customer")]

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int UserId = int.Parse(
         User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            

                var cartItem = await CartService.GetCartItem(UserId);

                return View(cartItem);
            

            
          
        }
        [Authorize(Roles = "Customer")]

        [HttpPost]
        public async Task<IActionResult> AddCustomerCart(int productId)
        {
            Debug.WriteLine($"Product ID: {productId}");
            int UserId = int.Parse(
         User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            try
            {
                await CartService.AddCart(UserId,productId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("Index","Cart");
            }
        }
        [Authorize(Roles = "Customer")]

        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(int itemId)
        {
            int UserId = int.Parse(
User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            try
            {
                await CartService.AddQuantity(UserId, itemId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ViewBag.Error = ex.Message;
                return RedirectToAction("Index");
            }

        }
        [Authorize(Roles = "Customer")]

        [HttpPost]
        public async Task<IActionResult> DeceraseQuantity(int itemId)
        {
            int UserId = int.Parse(
User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            try
            {
                await CartService.DeceraseQuantity(UserId, itemId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> RemoveItem(int itemIid)
        {
                        int UserId = int.Parse(
               User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            try
            {
                await CartService.RemoveItem(UserId, itemIid);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }





    }
}
