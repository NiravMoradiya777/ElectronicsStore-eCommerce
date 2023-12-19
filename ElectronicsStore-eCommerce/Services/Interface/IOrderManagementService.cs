using ElectronicsStore_eCommerce.Models;

namespace ElectronicsStore_eCommerce.Services.Interface
{
    // IOrderManagementService.cs
    public interface IOrderManagementService
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(int orderId);
        Task UpdateOrderStatusAsync(int orderId, string newStatus);
        Task GenerateInvoiceAsync(int orderId);
        Task ProcessReturnAsync(int orderId);
    }

}
