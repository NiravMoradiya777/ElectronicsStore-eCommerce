using System.Collections.Generic;

namespace ElectronicsStore_eCommerce.ViewModels
{
    public class ProductPageViewModel
    {
        public ProductDetailsViewModel ProductDetails { get; set; }
        public List<ProductViewModel> RelatedProducts { get; set; }
    }
}
