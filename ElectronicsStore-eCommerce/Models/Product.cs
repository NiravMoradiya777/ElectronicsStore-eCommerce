using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ElectronicsStore_eCommerce.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product description is required.")]
        [AllowHtml]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a valid price.")]
        public decimal Price { get; set; }

        public List<ProductImage>? Images { get; set; }

        public int CategoryId { get; set; }
        [BindNever]
        public Category? Category { get; set; }

        public bool IsFeatured { get; set; } = false;
        public bool IsSpecialOffer { get; set; } = false;
        public bool IsNewArrival { get; set; } = true;

        [BindNever]
        public List<Review>? Reviews { get; set; }

        // Additional properties
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
