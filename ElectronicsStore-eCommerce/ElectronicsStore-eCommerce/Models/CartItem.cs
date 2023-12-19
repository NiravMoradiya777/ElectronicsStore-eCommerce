using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsStore_eCommerce.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [BindNever]
        public Product? Product { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string ProductName { get; set; }

        [DataType(DataType.Currency)]
        public decimal ProductPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public decimal Subtotal { get; set; }

        public int CartId { get; set; }

        [BindNever]
        public Cart? Cart { get; set; }
    }
}
