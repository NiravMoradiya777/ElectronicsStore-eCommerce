using ElectronicsStore_eCommerce.Models;
using System.Collections.Generic;

namespace ElectronicsStore_eCommerce.ViewModels
{
    public class ProductDetailsViewModel
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<ProductImage> Images { get; set; }
        public List<Review> Reviews { get; set; }
        public List<ProductViewModel> RelatedProducts { get; set; }
    }
}
