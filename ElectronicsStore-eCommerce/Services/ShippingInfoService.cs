using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Models.ContextFile;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsStore_eCommerce.Services
{
    public class ShippingInfoService : IShippingInfoService
    {
        private readonly StoreDBContext _context;

        public ShippingInfoService(StoreDBContext context)
        {
            _context = context;
        }

        public async Task<List<ShippingInfo>> GetShippingInfoListByUserIdAsync(int userId)
        {
            return await _context.ShippingInfos
                .Where(info => info.UserId == userId)
                .ToListAsync();
        }

        public async Task<ShippingInfo> GetShippingInfoByIdAsync(int shippingInfoId)
        {
            return await _context.ShippingInfos
                .SingleOrDefaultAsync(info => info.ShippingInfoId == shippingInfoId);
        }

        public async Task AddShippingInfoAsync(int userId, ShippingInfo shippingInfo)
        {
            shippingInfo.UserId = userId;
            _context.ShippingInfos.Add(shippingInfo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateShippingInfoAsync(int shippingInfoId, ShippingInfo updatedShippingInfo)
        {
            var existingShippingInfo = await _context.ShippingInfos
                .SingleOrDefaultAsync(info => info.ShippingInfoId == shippingInfoId);

            if (existingShippingInfo != null)
            {
                // Update properties of the existing shipping info
                existingShippingInfo.ShippingAddress = updatedShippingInfo.ShippingAddress;
                existingShippingInfo.City = updatedShippingInfo.City;
                existingShippingInfo.ZipCode = updatedShippingInfo.ZipCode;
                existingShippingInfo.Country = updatedShippingInfo.Country;
                existingShippingInfo.State = updatedShippingInfo.State;
                existingShippingInfo.Region = updatedShippingInfo.Region;
                existingShippingInfo.IsDefaultShipping = updatedShippingInfo.IsDefaultShipping;
                existingShippingInfo.PhoneNumber = updatedShippingInfo.PhoneNumber;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteShippingInfoAsync(int shippingInfoId)
        {
            var shippingInfo = await _context.ShippingInfos
                .SingleOrDefaultAsync(info => info.ShippingInfoId == shippingInfoId);

            if (shippingInfo != null)
            {
                _context.ShippingInfos.Remove(shippingInfo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
