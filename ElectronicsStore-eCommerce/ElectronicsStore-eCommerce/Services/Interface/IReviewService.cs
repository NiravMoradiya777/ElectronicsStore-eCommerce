using ElectronicsStore_eCommerce.Models;

namespace ElectronicsStore_eCommerce.Services.Interface
{
    public interface IReviewService
    {
        Task<List<Review>> GetReviewsByProductIdAsync(int productId);
        Task AddReviewAsync(Review review);
    }
}
