using System.Security.Claims;
using System.Threading.Tasks;
using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicsStore_eCommerce.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IShoppingCartService _shoppingCartService;

        public OrderController(IOrderService orderService, IShoppingCartService shoppingCartService)
        {
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var orders = await _orderService.GetOrdersAsync(userId);

            return View(orders.Count > 0 ? orders : new List<Order>());
        }

        public async Task<IActionResult> Details(int orderId)
        {
            var userId = GetCurrentUserId();
            var order = await _orderService.GetOrderDetailsAsync(userId, orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int shippingInfoId)
        {
            var userId = GetCurrentUserId();
            var order = await _orderService.CreateOrderAsync(userId, shippingInfoId);

            // Optionally, you can provide feedback to the user, redirect to order summary, etc.
            return RedirectToAction("Details", new { orderId = order.OrderId });
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
