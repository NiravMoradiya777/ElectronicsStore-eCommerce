using ElectronicsStore_eCommerce.Models.ContextFile;
using ElectronicsStore_eCommerce.Models;
using Microsoft.EntityFrameworkCore;
using ElectronicsStore_eCommerce.Services.Interface;

namespace ElectronicsStore_eCommerce.Services
{
    // OrderManagementService.cs
    public class OrderManagementService : IOrderManagementService
    {
        private readonly StoreDBContext _context;

        public OrderManagementService(StoreDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders.Include(o => o.OrderItems).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.Include(o => o.User).Include(o => o.ShippingInfo).Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                await _context.SaveChangesAsync();
            }
        }

        public async Task GenerateInvoiceAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order != null)
            {
                // Here you would typically create an invoice document or update order status to "Invoice Generated"
                // For simplicity, let's just mark the order as invoiced
                order.Status = "Invoice Generated";
                await _context.SaveChangesAsync();
            }
        }

        public async Task ProcessReturnAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order != null)
            {
                // Here you would typically update inventory, create return documents, etc.
                // For simplicity, let's just mark the order as returned
                order.Status = "Returned";
                await _context.SaveChangesAsync();
            }
        }
    }

}
