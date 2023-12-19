using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore_eCommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imageService;
        private readonly ILogger<ProductController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductService productService, IImageService imageService, IWebHostEnvironment webHostEnvironment, ICategoryService categoryService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _imageService = imageService;
            _webHostEnvironment = webHostEnvironment;
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder, int page = 1, int pageSize = 10)
        {
            try
            {
                var products = await _productService.GetProductsAsync();

                if (!string.IsNullOrEmpty(searchString))
                {
                    products = products.Where(p => p.Name.Contains(searchString)).ToList();
                }

                TempData["CurrentFilter"] = searchString;

                // Apply sorting based on the sortOrder parameter
                switch (sortOrder)
                {
                    case "name_desc":
                        products = products.OrderByDescending(p => p.Name).ToList();
                        break;
                    case "price":
                        products = products.OrderBy(p => p.Price).ToList();
                        break;
                    case "price_desc":
                        products = products.OrderByDescending(p => p.Price).ToList();
                        break;
                    default:
                        products = products.OrderBy(p => p.Name).ToList();
                        break;
                }

                var paginatedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                TempData["Page"] = page;
                TempData["PageSize"] = pageSize;
                TempData["TotalPages"] = (int)Math.Ceiling((double)products.Count / pageSize);
                TempData["CurrentSort"] = sortOrder;

                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                TempData["ErrorMessage"] = "Error retrieving products. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Create()
        {
            try
            {
                ViewBag.Categories = _categoryService.GetCategoriesAsync().Result;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories for product creation");
                TempData["ErrorMessage"] = "Error retrieving categories for product creation. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.CreateProductAsync(product, imageFiles);
                    TempData["SuccessMessage"] = "Product created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (ApplicationException ex)
                {
                    _logger.LogError(ex, "Error creating product");
                    ModelState.AddModelError(string.Empty, "Error creating product. Please try again.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error creating product");
                    TempData["ErrorMessage"] = "An unexpected error occurred while creating the product. Please try again.";
                    return RedirectToAction(nameof(Index));
                }
            }

            try
            {
                ViewBag.Categories = await _categoryService.GetCategoriesAsync();
                TempData["ValidationErrors"] = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories for product creation");
                TempData["ErrorMessage"] = "Error retrieving categories for product creation. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                ViewBag.Categories = await _categoryService.GetCategoriesAsync();
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product for editing");
                TempData["ErrorMessage"] = "Error retrieving product for editing. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, List<IFormFile> newImageFiles)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _productService.GetProductByIdAsync(product.ProductId);

                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    // Update properties of the existing product with values from the submitted form
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.CategoryId = product.CategoryId;

                    await _productService.UpdateProductAsync(existingProduct, newImageFiles);

                    TempData["SuccessMessage"] = "Product updated successfully.";
                    return RedirectToAction("Index");
                }
                catch (ApplicationException ex)
                {
                    _logger.LogError(ex, "Error updating product");
                    ModelState.AddModelError(string.Empty, "Error updating product. Please try again.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error updating product");
                    TempData["ErrorMessage"] = "An unexpected error occurred while updating the product. Please try again.";
                    return RedirectToAction(nameof(Index));
                }
            }

            try
            {
                ViewBag.Categories = await _categoryService.GetCategoriesAsync();
                TempData["ValidationErrors"] = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories for product editing");
                TempData["ErrorMessage"] = "Error retrieving categories for product editing. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                TempData["SuccessMessage"] = "Product deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting product");
                TempData["ErrorMessage"] = "Error deleting product. Please try again.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting product");
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the product. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
