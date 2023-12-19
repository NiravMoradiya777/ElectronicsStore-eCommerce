// Controllers/HomeController.cs
using System;
using System.Threading.Tasks;
using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Models.ViewModel;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsStore_eCommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: /Home/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                // Retrieve featured products, special offers, new arrivals, and product categories
                var featuredProducts = await _productService.GetFeaturedProductsAsync();
                var specialOffers = await _productService.GetSpecialOffersAsync();
                var newArrivals = await _productService.GetNewArrivalsAsync();
                var productCategories = await _productService.GetProductCategoriesAsync();

                // You can customize the view model based on your requirements
                var viewModel = new HomePageViewModel
                {
                    FeaturedProducts = featuredProducts,
                    SpecialOffers = specialOffers,
                    NewArrivals = newArrivals,
                    ProductCategories = productCategories
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
