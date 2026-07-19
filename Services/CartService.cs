using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCDotnetCore.Data;
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

            var cartItem = await _context.CartItems.Include(X=>X.Cart).
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
                throw new Exception("Customer not found.");
            }
            var CustomerCart = await _context.Cart.Include(x => x.Customer).
               FirstOrDefaultAsync(x => x.Customer.UserId == UserId);
            var product = _context.Products.FirstOrDefault(x => x.Id == productId);
            var cartitems = await _context.CartItems.
                FirstOrDefaultAsync(x => x.ProductId == productId);
            if (CustomerCart != null  || cartitems!=null)
            {
                
                    var cartItem = new CartItem()
                    {
                        ProductId = productId,
                        Quantity = 1,
                        CartId = CustomerCart.Id,
                        UnitPrice = product.Price
                    };

                    _context.CartItems.Add(cartItem);
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
            }

            }
       public async  Task<CartItem> AddQuantity(int UserId, int itemId)
        {
            var Cart= _context.Cart.Include(c=>c.Customer).
                FirstOrDefault(x=>x.Customer.UserId == UserId);
            var cartitem = _context.CartItems.FirstOrDefault(x => x.Id == itemId
                && x.CartId == Cart.Id);
            Debug.WriteLine($"cartitem: {cartitem.Quantity}");
            Debug.WriteLine($"cartitemid: {cartitem.Id}");
            Debug.WriteLine($"cartitemproductid: {cartitem.ProductId}");
            Debug.WriteLine($"cartitemcartid: {cartitem.CartId}");



            cartitem.Quantity =1 + cartitem.Quantity;

            await _context.SaveChangesAsync();
            return cartitem;
        }
        public async Task<CartItem> DeceraseQuantity(int UserId,  int itemId)
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
       

    }
}


