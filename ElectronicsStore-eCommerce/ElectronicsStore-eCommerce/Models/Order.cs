using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsStore_eCommerce.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        [Required]
        public int UserId { get; set; }
        [BindNever]
        public User? User { get; set; }

        [Required(ErrorMessage = "Shipping info is required.")]
        public int ShippingInfoId { get; set; }
        [BindNever]
        public ShippingInfo? ShippingInfo { get; set; }

        [BindNever]
        public List<OrderItem>? OrderItems { get; set; }

        public String Status { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }
        // Additional properties as needed
    }
}