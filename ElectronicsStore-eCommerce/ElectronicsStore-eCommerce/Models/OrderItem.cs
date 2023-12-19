using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsStore_eCommerce.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [BindNever]
        public Product? Product { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public decimal Subtotal { get; set; }
        // Additional properties as needed

        [BindNever]
        public Order? Order { get; set; }
    }
}