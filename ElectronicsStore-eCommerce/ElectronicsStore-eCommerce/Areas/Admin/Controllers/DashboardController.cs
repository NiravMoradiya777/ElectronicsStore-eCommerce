// AdminDashboardController.cs
using ElectronicsStore_eCommerce.Models.ViewModel;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsStore_eCommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                TotalRevenue = await _dashboardService.GetTotalRevenueAsync(),
                TotalOrders = await _dashboardService.GetTotalOrdersAsync(),
                TopSellingProduct = await _dashboardService.GetTopSellingProductAsync(),
                TotalCategories = await _dashboardService.GetTotalCategoriesAsync(),
                MostPopularCategory = await _dashboardService.GetMostPopularCategoryAsync(),
                NewCategoriesAdded = await _dashboardService.GetNewCategoriesAddedAsync(),
                TotalProducts = await _dashboardService.GetTotalProductsAsync(),
                NewProductsAdded = await _dashboardService.GetNewProductsAddedAsync()
            };

            return View(model);
        }
    }
}
