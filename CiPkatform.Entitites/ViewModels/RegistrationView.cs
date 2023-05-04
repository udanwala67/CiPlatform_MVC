using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Entitites.ViewModels
{
    public class RegistrationView
    {
        [Required]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Minimum 6 characters are required")]
        public string Password { get; set; } = null!;
        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Minimum 10 characters are required")]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        /*[Display(Name = "Confirm Password")]*/
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
        public string ConfirmPassword { get; set; }
    }
}
