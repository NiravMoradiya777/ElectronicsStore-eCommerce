using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Services.Interface;

namespace ElectronicsStore_eCommerce.Services
{
    public class ImageService : IImageService
    {
        public async Task<string> SaveProductImageAsync(IFormFile imageFile)
        {
            // Implement the logic to save the image to your desired location
            // You might want to generate a unique filename, handle image resizing, etc.

            // For simplicity, let's assume you save the image to the wwwroot/images directory
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Return the relative URL of the saved image
            return "/images/" + uniqueFileName;
        }

        public async Task<List<ProductImage>> SaveProductImagesAsync(List<IFormFile> imageFiles)
        {
            var savedProductImages = new List<ProductImage>();

            foreach (var imageFile in imageFiles)
            {
                var savedImageUrl = await SaveProductImageAsync(imageFile);
                var productImage = new ProductImage { ImageUrl = savedImageUrl };
                savedProductImages.Add(productImage);
            }

            return savedProductImages;
        }
    }
}
