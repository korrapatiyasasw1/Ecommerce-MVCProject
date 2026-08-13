using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCDotnetCore.Data;
using MVCDotnetCore.Migrations;
using MVCDotnetCore.Models;
using System.Diagnostics;

namespace MVCDotnetCore.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;
        public CartService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Cart> GetCart(int UserId)
        {
            var cart = await _context.Cart.
                Include(x => x.Customer).ThenInclude(x => x.User).
   FirstOrDefaultAsync(x => x.Customer.UserId == UserId);
            if (cart == null)
            {
                throw new Exception("Cart is null");
            }
            else
            {
                return cart;
            }
        }
        public async Task<ViewModel> GetCartItem(int UserId)
        {

            var cartItem = await _context.CartItems.Include(X => X.Cart).
                ThenInclude(x => x.Customer).ThenInclude(x => x.User).
   FirstOrDefaultAsync(x => x.Cart.Customer.UserId == UserId);


            if (cartItem == null)
            {
                return null;
            }


            var cartItems = await _context.CartItems.Include(x => x.Product).
                Where(x => x.CartId == cartItem.Cart.Id).ToListAsync();
            int grandtotal = 0;

            foreach (var item in cartItems)
            {
                grandtotal = grandtotal + ((int)item.UnitPrice * item.Quantity);
            }
            return new ViewModel
            {
                CartItems = cartItems,
                GrandTotal = grandtotal
            };


        }
        public async Task AddCart(int UserId, int productId)
        {

            var customer = await _context.Customers.Include(x => x.User).
               FirstOrDefaultAsync(x => x.UserId == UserId);
            if (customer == null)
            {
                throw new Exception("Before adding the products please fill the User Profile .");
            }
            var CustomerCart = await _context.Cart.Include(x => x.Customer).
               FirstOrDefaultAsync(x => x.Customer.UserId == UserId);
            var product = _context.Products.FirstOrDefault(x => x.Id == productId);
            var cartitems = await _context.CartItems.
                FirstOrDefaultAsync(x => x.ProductId == productId);
            if (CustomerCart != null || cartitems != null)
            {

                var cartItem = new Models.CartItem()
                {
                    ProductId = productId,
                    Quantity = 1,
                    CartId = CustomerCart.Id,
                    UnitPrice = product.Price
                };

                _context.CartItems.Add(cartItem);
                Debug.WriteLine($"{cartItem.Cart.CustomerId} ");

                await _context.SaveChangesAsync();


            }

            else
            {
                var cart = new Cart();
                {
                    cart.CustomerId = customer.Id;
                    cart.CreatedDate = DateTime.Now;
                }

                _context.Cart.Add(cart);


                await _context.SaveChangesAsync();
                Debug.WriteLine($"{cart.CustomerId} and {cart.Id}");

            }

        }
    
       public async  Task<Models.CartItem> AddQuantity(int UserId, int itemId)
        {
            var Cart= _context.Cart.Include(c=>c.Customer).
                FirstOrDefault(x=>x.Customer.UserId == UserId);
            var cartitem = _context.CartItems.
                FirstOrDefault(x => x.Id == itemId
                && x.CartId == Cart.Id);
   

            cartitem.Quantity = 1 + cartitem.Quantity;

            await _context.SaveChangesAsync();
            return cartitem;
        }
        public async Task<Models.CartItem> DeceraseQuantity(int UserId,  int itemId)
        {
            var Cart = _context.Cart.Include(c => c.Customer).
                FirstOrDefault(x => x.Customer.UserId == UserId);
            var cartitem = _context.CartItems.FirstOrDefault(x => x.Id == itemId
                && x.CartId == Cart.Id);
            if (cartitem.Quantity > 1)
            {
                cartitem.Quantity = cartitem.Quantity - 1;
                await _context.SaveChangesAsync();
            }
            else if(cartitem.Quantity == 1)
            {
                _context.CartItems.Remove(cartitem);
                await _context.SaveChangesAsync();
            }
            return cartitem;

        }
        public async Task<List<Models.CartItem>> RemoveItem(int UserId, int itemIid)
        {
            var Cart = _context.Cart
                       .Include(c => c.Customer)
                       .FirstOrDefault(x => x.Customer.UserId ==
                       UserId);
            var cartitem =await _context.CartItems
                           .Where(x => x.Id == itemIid
                            && x.CartId == Cart.Id).ToListAsync();
           

                foreach (var item in cartitem)
                {

                    _context.CartItems.Remove(item);
                }
            
            
            await _context.SaveChangesAsync();
            var cartitems= await _context.CartItems
                           .Where(x=>x.CartId == Cart.Id)
                           .ToListAsync();
            if (cartitem == null)
            {
                return null;
            }
            return cartitems;
            

        }



    }
}


