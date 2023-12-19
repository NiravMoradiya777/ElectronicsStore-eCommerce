using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Services.Interface;
using ElectronicsStore_eCommerce.ViewModels;

namespace ElectronicsStore_eCommerce.Services
{
    public class ProductPageService : IProductPageService
    {
        private readonly IProductService _productService;
        private readonly IReviewService _reviewService;

        public ProductPageService(IProductService productService, IReviewService reviewService)
        {
            _productService = productService;
            _reviewService = reviewService;
        }

        public async Task<ProductDetailsViewModel> GetProductDetailsAsync(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            var reviews = await _reviewService.GetReviewsByProductIdAsync(productId);
            var relatedProduct = await GetRelatedProductsAsync(product.CategoryId, productId);

            var productViewModel = new ProductDetailsViewModel
            {
                ProductId = product.ProductId,
                CategoryId = product.CategoryId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Images = product.Images,
                Reviews = reviews,
                RelatedProducts = relatedProduct
            };

            return productViewModel;
        }

        public async Task<List<ProductViewModel>> GetRelatedProductsAsync(int categoryId, int productId)
        {
            var relatedProducts = await _productService.GetProductsByCategoryAsync(categoryId);

            // Exclude the current product from related products
            relatedProducts = relatedProducts.Where(p => p.ProductId != productId).ToList();

            var relatedProductViewModels = relatedProducts.Select(p => new ProductViewModel
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.Images[0].ImageUrl
            }).ToList();

            return relatedProductViewModels;
        }
    }
}
