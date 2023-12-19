using ElectronicsStore_eCommerce.Models;

namespace ElectronicsStore_eCommerce.Services.Interface
{
    public interface IImageService
    {
        Task<string> SaveProductImageAsync(IFormFile imageFile);
        Task<List<ProductImage>> SaveProductImagesAsync(List<IFormFile> imageFiles);
    }
}
