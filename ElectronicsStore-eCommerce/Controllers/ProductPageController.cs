using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.ViewModels;
using ElectronicsStore_eCommerce.Services.Interface;

public class ProductPageController : Controller
{
    private readonly IProductPageService _productPageService;

    public ProductPageController(IProductPageService productPageService)
    {
        _productPageService = productPageService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int id)
    {
        if (id == 0) {
            return null;
        }
        var productDetails = await _productPageService.GetProductDetailsAsync(id);

        if (productDetails == null)
        {
            return NotFound();
        }

        return View(productDetails);
    }
}
