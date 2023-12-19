namespace ElectronicsStore_eCommerce.Models.ViewModel
{
    public class DashboardViewModel
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public string TopSellingProduct { get; set; }

        public int TotalCategories { get; set; }
        public string MostPopularCategory { get; set; }
        public int NewCategoriesAdded { get; set; }

        public int TotalProducts { get; set; }
        public int NewProductsAdded { get; set; }
    }

}
