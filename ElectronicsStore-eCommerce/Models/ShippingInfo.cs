using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsStore_eCommerce.Models
{
    public class ShippingInfo
    {
        public int ShippingInfoId { get; set; }

        [Required(ErrorMessage = "Shipping address is required.")]
        [StringLength(255, ErrorMessage = "Shipping address must not exceed 255 characters.")]
        public string ShippingAddress { get; set; }

        [StringLength(100, ErrorMessage = "City must not exceed 100 characters.")]
        public string City { get; set; }

        [StringLength(20, ErrorMessage = "Zip code must not exceed 20 characters.")]
        public string ZipCode { get; set; }

        // Other shipping-related properties
        [StringLength(50, ErrorMessage = "Country must not exceed 50 characters.")]
        public string Country { get; set; }

        [StringLength(50, ErrorMessage = "State must not exceed 50 characters.")]
        public string State { get; set; }

        [StringLength(50, ErrorMessage = "Region must not exceed 50 characters.")]
        public string Region { get; set; }

        public bool IsDefaultShipping { get; set; }

        // Additional properties
        [StringLength(20, ErrorMessage = "Phone number must not exceed 20 characters.")]
        public string PhoneNumber { get; set; }

        // Foreign Key
        public int UserId { get; set; }
        [BindNever]
        public User? User { get; set; }

        // Orders
        [BindNever]
        public ICollection<Order>? Orders { get; set; }
    }
}
