using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ElectronicsStore_eCommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string nameFilter, int? categoryFilter, string sortBy)
        {
            var products = await _productService.GetProductsAsync(nameFilter, categoryFilter, sortBy);

            // Optionally, you can pass additional data to the view, such as the list of categories
            ViewBag.Categories = await _categoryService.GetCategoriesAsync();

            return View(products);
        }

        public async Task<IActionResult> GetProducts(string nameFilter, int? categoryFilter, string sortBy)
        {
            var products = await _productService.GetProductsAsync(nameFilter, categoryFilter, sortBy);
            return PartialView("_ProductListPartial", products);
        }
    }
}
