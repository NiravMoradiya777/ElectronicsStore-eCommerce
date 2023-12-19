namespace ElectronicsStore_eCommerce.Models.ViewModel
{
    public class HomePageViewModel
    {
        public List<Product> FeaturedProducts { get; set; }
        public List<Product> SpecialOffers { get; set; }
        public List<Product> NewArrivals { get; set; }
        public List<Category> ProductCategories { get; set; }
    }
}
