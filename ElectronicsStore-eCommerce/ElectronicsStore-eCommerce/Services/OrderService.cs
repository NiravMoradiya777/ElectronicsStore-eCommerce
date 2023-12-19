using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Models.ContextFile;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsStore_eCommerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly StoreDBContext _context;
        private readonly IShoppingCartService _shoppingCartService;

        public OrderService(StoreDBContext context, IShoppingCartService shoppingCartService)
        {
            _context = context;
            _shoppingCartService = shoppingCartService;
        }

        public async Task<List<Order>> GetOrdersAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> GetOrderDetailsAsync(int userId, int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.OrderId == orderId);
        }

        public async Task<Order> CreateOrderAsync(int userId, int shippingInfoId)
        {
            var cart = await _shoppingCartService.GetCartAsync(userId);

            var order = new Order
            {
                UserId = userId,
                ShippingInfoId = shippingInfoId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending", // Set the initial status as needed
                OrderItems = cart.CartItems.Select(cartItem => new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Subtotal = cartItem.Subtotal
                }).ToList(),
                TotalAmount = cart.TotalAmount
                // Add other properties as needed
            };

            // Save the order to the database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Clear the shopping cart
            await _shoppingCartService.ClearCartAsync(userId);

            return order;
        }

    }
}
