using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ElectronicsStore_eCommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imageService;
        private readonly ILogger<CategoryController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(ICategoryService categoryService, IImageService imageService, IWebHostEnvironment webHostEnvironment, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _imageService = imageService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            try
            {
                var categories = await _categoryService.GetCategoriesAsync();

                if (!string.IsNullOrEmpty(searchString))
                {
                    categories = categories.Where(c => c.Name.Contains(searchString)).ToList();
                }

                ViewBag.CurrentFilter = searchString;

                // Display success message if available
                ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;

                return View(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories");
                TempData["ErrorMessage"] = "An error occurred while retrieving categories. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Create()
        {
            try
            {
                var parentCategories = _categoryService.GetCategoriesAsync().Result;
                ViewBag.ParentCategories = parentCategories;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving parent categories for create");
                TempData["ErrorMessage"] = "An error occurred while retrieving parent categories. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (file != null && file.Length > 0)
                    {
                        category.ImageUrl = await _imageService.SaveProductImageAsync(file);
                    }

                    await _categoryService.CreateCategoryAsync(category);

                    TempData["SuccessMessage"] = "Category created successfully!";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.ParentCategories = await _categoryService.GetCategoriesAsync();
                ViewBag.ValidationErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["ErrorMessage"] = "Please correct the validation errors.";
                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                TempData["ErrorMessage"] = "An error occurred while creating the category. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                var parentCategories = await _categoryService.GetCategoriesAsync();

                ViewBag.ParentCategories = parentCategories;

                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving category with ID {id} for edit");
                TempData["ErrorMessage"] = $"An error occurred while retrieving the category for editing. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category, IFormFile? imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingCategory = await _categoryService.GetCategoryByIdAsync(category.CategoryId);

                    if (existingCategory == null)
                    {
                        return NotFound();
                    }

                    existingCategory.Name = category.Name;
                    existingCategory.Description = category.Description;
                    existingCategory.ParentCategoryId = category.ParentCategoryId;

                    if (imageFile != null)
                    {
                        existingCategory.ImageUrl = await _imageService.SaveProductImageAsync(imageFile);
                    }

                    await _categoryService.UpdateCategoryAsync(existingCategory);

                    TempData["SuccessMessage"] = "Category updated successfully!";
                    return RedirectToAction(nameof(Index));
                }

                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating category with ID {category.CategoryId}");
                TempData["ErrorMessage"] = "An error occurred while updating the category. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                TempData["SuccessMessage"] = "Category deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting category with ID {id}");
                TempData["ErrorMessage"] = "An error occurred while deleting the category. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
