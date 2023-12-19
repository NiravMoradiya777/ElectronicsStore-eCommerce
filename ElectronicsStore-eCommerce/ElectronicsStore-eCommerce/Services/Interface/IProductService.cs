using ElectronicsStore_eCommerce.Models;

namespace ElectronicsStore_eCommerce.Services.Interface
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task CreateProductAsync(Product product, List<IFormFile> imageFiles);
        Task UpdateProductAsync(Product product, List<IFormFile> newImageFiles);
        Task DeleteProductAsync(int id);

        Task<List<Product>> GetFeaturedProductsAsync();
        Task<List<Product>> GetSpecialOffersAsync();
        Task<List<Product>> GetNewArrivalsAsync();
        Task<List<Category>> GetProductCategoriesAsync();
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);

        Task<List<Product>> GetProductsAsync(string nameFilter, int? categoryFilter, string sortBy);
    }
}
