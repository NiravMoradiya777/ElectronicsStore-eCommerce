using ElectronicsStore_eCommerce.ViewModels;

namespace ElectronicsStore_eCommerce.Services.Interface
{
    public interface IProductPageService
    {
        Task<ProductDetailsViewModel> GetProductDetailsAsync(int productId);
        Task<List<ProductViewModel>> GetRelatedProductsAsync(int categoryId, int productId);
    }
}
