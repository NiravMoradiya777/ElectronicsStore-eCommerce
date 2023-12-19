using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsStore_eCommerce.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        [BindNever]
        public List<CartItem>? CartItems { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }
        // Additional properties as needed

        [Required(ErrorMessage = "User is required.")]
        public int UserId { get; set; }
        [BindNever]
        public User? User { get; set; }
    }
}
