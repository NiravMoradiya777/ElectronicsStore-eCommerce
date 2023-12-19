using System;

namespace ElectronicsStore_eCommerce.ViewModels
{
    public class ProductReviewViewModel
    {
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
