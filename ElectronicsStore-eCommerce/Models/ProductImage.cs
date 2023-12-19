using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsStore_eCommerce.Models
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }

        [Required(ErrorMessage = "Image URL is required.")]
        public string ImageUrl { get; set; }

        public int ProductId { get; set; }
        [BindNever]
        public Product? Product { get; set; }
    }
}
