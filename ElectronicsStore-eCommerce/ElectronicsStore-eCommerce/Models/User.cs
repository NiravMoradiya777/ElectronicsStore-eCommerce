using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicsStore_eCommerce.Models
{
    public class User : IdentityUser<int>
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name must be at most 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name must be at most 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public override string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public override string PasswordHash { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("PasswordHash", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [NotMapped] // NotMapped prevents this property from being mapped to the database
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthdate { get; set; }

        [Required(ErrorMessage = "Admin status is required.")]
        public bool IsAdmin { get; set; } = false;

        // Personal Info
        [BindNever]
        public ICollection<ShippingInfo>? ShippingInfos { get; set; }

        // Orders
        [BindNever]
        public ICollection<Order>? Orders { get; set; }

        // Cart
        [BindNever]
        public ICollection<Cart>? Cart { get; set; }
    }
}
