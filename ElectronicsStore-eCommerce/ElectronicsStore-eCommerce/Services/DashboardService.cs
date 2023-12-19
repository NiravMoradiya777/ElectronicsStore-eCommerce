// DashboardService.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using ElectronicsStore_eCommerce.Models.ContextFile;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.EntityFrameworkCore;

public class DashboardService : IDashboardService
{
    private readonly StoreDBContext _context;

    public DashboardService(StoreDBContext context)
    {
        _context = context;
    }

    public async Task<decimal> GetTotalRevenueAsync()
    {
        return await _context.Orders.SumAsync(o => o.TotalAmount);
    }

    public async Task<int> GetTotalOrdersAsync()
    {
        return await _context.Orders.CountAsync();
    }

    public async Task<string> GetTopSellingProductAsync()
    {
        var topSellingProduct = await _context.OrderItems
            .GroupBy(oi => oi.ProductId)
            .OrderByDescending(group => group.Sum(oi => oi.Quantity))
            .Select(group => group.Key)
            .FirstOrDefaultAsync();

        var productName = await _context.Products
            .Where(p => p.ProductId == topSellingProduct)
            .Select(p => p.Name)
            .FirstOrDefaultAsync();

        return productName ?? "N/A";
    }


    public async Task<int> GetTotalCategoriesAsync()
    {
        return await _context.Categories.CountAsync();
    }

    public async Task<string> GetMostPopularCategoryAsync()
    {
        var mostPopularCategoryId = await _context.Products
            .GroupBy(p => p.CategoryId)
            .OrderByDescending(group => group.Count())
            .Select(group => group.Key)
            .FirstOrDefaultAsync();

        var categoryName = await _context.Categories
            .Where(c => c.CategoryId == mostPopularCategoryId)
            .Select(c => c.Name)
            .FirstOrDefaultAsync();

        return categoryName ?? "N/A";
    }


    public async Task<int> GetNewCategoriesAddedAsync()
    {
        var last30Days = DateTime.UtcNow.AddDays(-30);
        return await _context.Categories.CountAsync(c => c.CreatedAt >= last30Days);
    }

    public async Task<int> GetTotalProductsAsync()
    {
        return await _context.Products.CountAsync();
    }

    public async Task<int> GetNewProductsAddedAsync()
    {
        var last30Days = DateTime.UtcNow.AddDays(-30);
        return await _context.Products.CountAsync(p => p.CreatedAt >= last30Days);
    }
}
