using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElectronicsStore_eCommerce.Controllers
{
    [Authorize]
    public class ShippingInfoController : Controller
    {
        private readonly IShippingInfoService _shippingInfoService;

        public ShippingInfoController(IShippingInfoService shippingInfoService)
        {
            _shippingInfoService = shippingInfoService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = GetCurrentUserId();
                var shippingInfoList = await _shippingInfoService.GetShippingInfoListByUserIdAsync(userId);

                return View(shippingInfoList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error retrieving shipping information: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ShippingInfo shippingInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = GetCurrentUserId();
                    await _shippingInfoService.AddShippingInfoAsync(userId, shippingInfo);

                    TempData["SuccessMessage"] = "Shipping information added successfully.";
                    return RedirectToAction("Index");
                }

                TempData["ValidationErrors"] = "Please correct the validation errors.";
                return View(shippingInfo);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error creating shipping information: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var shippingInfo = await _shippingInfoService.GetShippingInfoByIdAsync(id);

                if (shippingInfo == null)
                {
                    return NotFound();
                }

                return View(shippingInfo);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error retrieving shipping information: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ShippingInfo updatedShippingInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _shippingInfoService.UpdateShippingInfoAsync(id, updatedShippingInfo);

                    TempData["SuccessMessage"] = "Shipping information updated successfully.";
                    return RedirectToAction("Index");
                }

                TempData["ValidationErrors"] = "Please correct the validation errors.";
                return View(updatedShippingInfo);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating shipping information: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _shippingInfoService.DeleteShippingInfoAsync(id);

                TempData["SuccessMessage"] = "Shipping information deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting shipping information: {ex.Message}";
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
