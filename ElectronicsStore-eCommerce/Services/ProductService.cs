using ElectronicsStore_eCommerce.Models.ContextFile;
using ElectronicsStore_eCommerce.Models;
using Microsoft.EntityFrameworkCore;
using ElectronicsStore_eCommerce.Services.Interface;

namespace ElectronicsStore_eCommerce.Services
{
    public class ProductService : IProductService
    {
        private readonly StoreDBContext _context;
        private readonly IImageService _imageService; // Assuming you have an image service

        public ProductService(StoreDBContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products.Include(p => p.Images).Include(p => p.Category).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task CreateProductAsync(Product product, List<IFormFile> imageFiles)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();

                    // Save product images
                    if (imageFiles != null && imageFiles.Any())
                    {
                        foreach (var file in imageFiles)
                        {
                            var imageUrl = await _imageService.SaveProductImageAsync(file);

                            var productImage = new ProductImage
                            {
                                ImageUrl = imageUrl,
                                ProductId = product.ProductId
                            };

                            _context.ProductImages.Add(productImage);
                        }

                        await _context.SaveChangesAsync();
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task UpdateProductAsync(Product product, List<IFormFile> newImageFiles)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();

                    // Remove existing images
                    var existingImages = await _context.ProductImages
                        .Where(pi => pi.ProductId == product.ProductId)
                        .ToListAsync();

                    // Save new product images
                    if (newImageFiles != null && newImageFiles.Any() && newImageFiles.Count > 0)
                    {
                        _context.ProductImages.RemoveRange(existingImages);
                        foreach (var file in newImageFiles)
                        {
                            var imageUrl = await _imageService.SaveProductImageAsync(file);

                            var productImage = new ProductImage
                            {
                                ImageUrl = imageUrl,
                                ProductId = product.ProductId
                            };

                            _context.ProductImages.Add(productImage);
                        }

                        await _context.SaveChangesAsync();
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetFeaturedProductsAsync()
        {
            return await _context.Products.Where(p => p.IsFeatured).Include(p => p.Images).Take(3).ToListAsync();
        }

        public async Task<List<Product>> GetSpecialOffersAsync()
        {
            return await _context.Products.Where(p => p.IsSpecialOffer).Include(p => p.Images).Take(3).ToListAsync();
        }

        public async Task<List<Product>> GetNewArrivalsAsync()
        {
            return await _context.Products.Where(p => p.IsNewArrival).Include(p => p.Images).Take(3).ToListAsync();
        }

        public async Task<List<Category>> GetProductCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Images)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsAsync(string nameFilter, int? categoryFilter, string sortBy)
        {
            var query = _context.Products
                .Include(p => p.Images)
                .Include(p => p.Category) // Assuming you have a navigation property for the category
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(nameFilter))
            {
                query = query.Where(p => p.Name.ToLower().Contains(nameFilter.ToLower()));
            }

            if (categoryFilter.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryFilter);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = ApplySorting(query, sortBy);
            }

            // Project the results into DTOs
            var products = await query.Include(p => p.Images).Include(p => p.Category).ToListAsync();

            return products;
        }

        // Helper method for applying sorting
        private IQueryable<Product> ApplySorting(IQueryable<Product> query, string sortBy)
        {
            // Example: sortBy can be "Name" or "Price" or any other property you want to sort by
            switch (sortBy.ToLower())
            {
                case "name":
                    return query.OrderBy(p => p.Name);
                case "price":
                    return query.OrderBy(p => p.Price);
                // Add more cases for additional sorting options
                default:
                    return query;
            }
        }
    }
}
