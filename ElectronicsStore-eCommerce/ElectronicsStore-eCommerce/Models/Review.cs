using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ElectronicsStore_eCommerce.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "Review text is required.")]
        public string Text { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        public int ProductId { get; set; }
        [BindNever]
        public Product? Product { get; set; }

        public string UserId { get; set; } // Foreign key for user
        [BindNever]
        public User? User { get; set; }

        // Additional properties
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
