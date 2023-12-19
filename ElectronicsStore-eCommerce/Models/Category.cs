using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsStore_eCommerce.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        // For hierarchical structure
        public int? ParentCategoryId { get; set; } = 0;
        [BindNever]
        public Category? ParentCategory { get; set; }

        // For relationships
        [BindNever]
        public ICollection<Product>? Products { get; set; }

        // Additional properties
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Other properties as needed

        // Constructor for initializing collections
        public Category()
        {
            Products = new List<Product>();
        }
    }
}
