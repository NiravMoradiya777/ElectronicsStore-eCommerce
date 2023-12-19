// OrderManagementController.cs
using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore_eCommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderManagementController : Controller
    {
        private readonly IOrderManagementService _orderManagementService;

        public OrderManagementController(IOrderManagementService orderManagementService)
        {
            _orderManagementService = orderManagementService;
        }

        public async Task<IActionResult> Index(string statusFilter, string sortField, string sortOrder)
        {
            try
            {
                var orders = await _orderManagementService.GetOrdersAsync();

                if (!string.IsNullOrEmpty(statusFilter))
                {
                    // Apply the status filter if provided
                    orders = orders.Where(o => o.Status == statusFilter).ToList();
                }

                // Apply sorting based on the selected criteria
                if (!string.IsNullOrEmpty(sortField))
                {
                    switch (sortField)
                    {
                        case "OrderDate":
                            orders = sortOrder == "Asc" ? orders.OrderBy(o => o.OrderDate).ToList() : orders.OrderByDescending(o => o.OrderDate).ToList();
                            break;
                        case "TotalAmount":
                            orders = sortOrder == "Asc" ? orders.OrderBy(o => o.TotalAmount).ToList() : orders.OrderByDescending(o => o.TotalAmount).ToList();
                            break;
                            // Add other sorting options as needed
                    }
                }

                return View(orders);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View(new List<Order>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var order = await _orderManagementService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }

                return View(order);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return View(new Order());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string newStatus)
        {
            try
            {
                await _orderManagementService.UpdateOrderStatusAsync(orderId, newStatus);
                ViewBag.SuccessMessage = "Order status updated successfully.";
                return RedirectToAction("Details", new { id = orderId });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return RedirectToAction("Details", new { id = orderId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateInvoice(int orderId)
        {
            try
            {
                await _orderManagementService.GenerateInvoiceAsync(orderId);
                ViewBag.SuccessMessage = "Invoice generated successfully.";
                return RedirectToAction("Details", new { id = orderId });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return RedirectToAction("Details", new { id = orderId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessReturn(int orderId)
        {
            try
            {
                await _orderManagementService.ProcessReturnAsync(orderId);
                ViewBag.SuccessMessage = "Return processed successfully.";
                return RedirectToAction("Details", new { id = orderId });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                return RedirectToAction("Details", new { id = orderId });
            }
        }
    }
}
