using System.Threading.Tasks;
using ElectronicsStore_eCommerce.Models;

namespace ElectronicsStore_eCommerce.Services.Interface
{
    public interface IShoppingCartService
    {
        Task<Cart> GetCartAsync(int userId);
        Task AddToCartAsync(int userId, int productId, int quantity);
        Task UpdateCartAsync(Cart cart);
        Task RemoveFromCartAsync(int userId, int productId);
        Task ClearCartAsync(int userId);
        Task UpdateCartQTY(int CartItemId, int Quantity);
    }
}
