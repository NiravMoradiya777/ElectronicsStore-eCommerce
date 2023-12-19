using ElectronicsStore_eCommerce.Models;

namespace ElectronicsStore_eCommerce.Services.Interface
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersAsync(int userId);
        Task<Order> GetOrderDetailsAsync(int userId, int orderId);
        Task<Order> CreateOrderAsync(int userId, int shippingInfoId);
    }
}
