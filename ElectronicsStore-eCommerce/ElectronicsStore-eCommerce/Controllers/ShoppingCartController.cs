using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsStore_eCommerce.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = GetCurrentUserId();
                var cart = await _shoppingCartService.GetCartAsync(userId);

                return View(cart);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _shoppingCartService.AddToCartAsync(userId, productId, quantity);

                TempData["SuccessMessage"] = "Product added to the cart successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCart(Cart cart)
        {
            try
            {
                await _shoppingCartService.UpdateCartAsync(cart);

                TempData["SuccessMessage"] = "Cart updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _shoppingCartService.RemoveFromCartAsync(userId, productId);

                TempData["SuccessMessage"] = "Product removed from the cart successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCartQTY(int CartItemId, int Quantity)
        {
            try
            {
                await _shoppingCartService.UpdateCartQTY(CartItemId, Quantity);

                TempData["SuccessMessage"] = "Cart quantity updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        private int GetCurrentUserId()
        {
            // Get the current user's ID using ASP.NET Core Identity
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out var result))
            {
                return result;
            }

            return 0;
        }
    }
}
