using ElectronicsStore_eCommerce.Models;

namespace ElectronicsStore_eCommerce.Services.Interface
{
    public interface IShippingInfoService
    {
        Task<List<ShippingInfo>> GetShippingInfoListByUserIdAsync(int userId);
        Task<ShippingInfo> GetShippingInfoByIdAsync(int shippingInfoId);
        Task AddShippingInfoAsync(int userId, ShippingInfo shippingInfo);
        Task UpdateShippingInfoAsync(int shippingInfoId, ShippingInfo updatedShippingInfo);
        Task DeleteShippingInfoAsync(int shippingInfoId);
    }
}
