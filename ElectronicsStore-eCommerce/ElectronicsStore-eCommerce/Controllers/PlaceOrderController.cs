using ElectronicsStore_eCommerce.Models.ViewModel;
using ElectronicsStore_eCommerce.Services;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElectronicsStore_eCommerce.Controllers
{
    [Authorize]
    public class PlaceOrderController : Controller
    {
        private readonly IShippingInfoService _shippingInfoService;
        private readonly IOrderService _orderService;

        public PlaceOrderController(IShippingInfoService shippingInfoService, IOrderService orderService)
        {
            _shippingInfoService = shippingInfoService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = GetCurrentUserId();
                var availableShippingInfos = await _shippingInfoService.GetShippingInfoListByUserIdAsync(userId);
                var availablePaymentMethods = GetAvailablePaymentMethods();

                var viewModel = new PlaceOrderViewModel
                {
                    AvailableShippingInfos = availableShippingInfos,
                    AvailablePaymentMethods = availablePaymentMethods
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error retrieving data: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(PlaceOrderViewModel viewModel)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (!ModelState.IsValid)
                {
                    TempData["ValidationErrors"] = "Please correct the validation errors.";
                    return View(viewModel);
                }

                // Process the order asynchronously based on the selected shipping information and payment method
                // Implement the logic to place the order
                await _orderService.CreateOrderAsync(userId, viewModel.SelectedShippingInfoId);

                // Simulate asynchronous processing with a delay
                await Task.Delay(2000);

                TempData["SuccessMessage"] = "Order placed successfully!";
                return RedirectToAction("OrderConfirmation"); // Redirect to a confirmation page
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error placing order: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }

        private List<string> GetAvailablePaymentMethods()
        {
            return new List<string> { "Credit Card", "PayPal", "Cash on Delivery" };
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
