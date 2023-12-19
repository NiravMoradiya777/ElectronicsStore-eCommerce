// IDashboardService.cs
using System.Threading.Tasks;

namespace ElectronicsStore_eCommerce.Services.Interface
{
    public interface IDashboardService
    {
        Task<decimal> GetTotalRevenueAsync();
        Task<int> GetTotalOrdersAsync();
        Task<string> GetTopSellingProductAsync();
        Task<int> GetTotalCategoriesAsync();
        Task<string> GetMostPopularCategoryAsync();
        Task<int> GetNewCategoriesAddedAsync();
        Task<int> GetTotalProductsAsync();
        Task<int> GetNewProductsAddedAsync();
    }
}
