using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Models.ContextFile;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.Build.Experimental.ProjectCache;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsStore_eCommerce.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly StoreDBContext _dbContext;

        public ShoppingCartService(StoreDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Cart> GetCartAsync(int userId)
        {
            return await _dbContext.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product).ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddToCartAsync(int userId, int productId, int quantity)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var product = await _dbContext.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    Quantity = quantity
                };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            cartItem.Subtotal = product.Price * cartItem.Quantity;
            cart.TotalAmount += cartItem.Subtotal;

            await UpdateCartAsync(cart);
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _dbContext.Carts.Update(cart);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(int userId, int productId)
        {
            var cart = await GetCartAsync(userId);

            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

                if (cartItem != null)
                {
                    cart.CartItems.Remove(cartItem);
                    await UpdateCartAsync(cart);
                }
            }
        }

        private async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            var cart = await _dbContext.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                _dbContext.Carts.Add(cart);
                await _dbContext.SaveChangesAsync();
            }

            return cart;
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await GetCartAsync(userId);

            if (cart != null)
            {
                var cartItems = cart.CartItems.ToList();
                foreach (var cartItem in cartItems) {
                    cart.CartItems.Remove(cartItem);
                }
                await UpdateCartAsync(cart);
            }
        }

        public async Task UpdateCartQTY(int CartItemId, int Quantity)
        {
            var cartItem = await _dbContext.CartItems.FindAsync(CartItemId);

            if (cartItem != null)
            {
                var newQTY = Quantity - cartItem.Quantity;
                var priceDiff = newQTY * cartItem.ProductPrice;
                cartItem.Quantity = Quantity;
                cartItem.Subtotal += priceDiff;

                _dbContext.CartItems.Update(cartItem);

                var cart = await _dbContext.Carts.FindAsync(cartItem.CartId);
                if (cart != null)
                {
                    cart.TotalAmount += priceDiff;
                    _dbContext.Carts.Update(cart);
                }
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
