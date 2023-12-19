using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsStore_eCommerce.Controllers
{
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public CategoryListViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _productService.GetProductCategoriesAsync().Result;
            return View(categories);
        }
    }
}
