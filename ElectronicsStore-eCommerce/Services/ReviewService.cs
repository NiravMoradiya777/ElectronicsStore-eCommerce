using ElectronicsStore_eCommerce.Models;
using ElectronicsStore_eCommerce.Models.ContextFile;
using ElectronicsStore_eCommerce.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsStore_eCommerce.Services
{
    public class ReviewService : IReviewService
    {
        private readonly StoreDBContext _dbContext;

        public ReviewService(StoreDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Review>> GetReviewsByProductIdAsync(int productId)
        {
            return await _dbContext.Reviews
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }

        public async Task AddReviewAsync(Review review)
        {
            _dbContext.Reviews.Add(review);
            await _dbContext.SaveChangesAsync();
        }
    }
}
