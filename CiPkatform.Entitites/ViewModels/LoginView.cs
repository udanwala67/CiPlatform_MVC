using CiPlatform.Entitites.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Entitites.ViewModels
{
    public class LoginView
    {
        /*public List<User> user = new List<User>();*/
        /* public long UserId { get; set; }
         public string? FirstName { get; set; }
         public string? LastName { get; set; }*/
        [Required]
        public string Email { get; set; } = null!;
        
        [Required]
        
      /*  [StringLength(100, MinimumLength = 6, ErrorMessage = "Minimum 6 characters are required")]*/
        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;
        /* public int PhoneNumber { get; set; }
         public string? Avatar { get; set; }
         public string? WhyIVolunteer { get; set; }
         public string? EmployeeId { get; set; }
         public string? Department { get; set; }
         public long CityId { get; set; }
         public long CountryId { get; set; }
         public string? ProfileText { get; set; }
         public string? LinkedInUrl { get; set; }
         public string? Title { get; set; }
         public int Status { get; set; }
         public DateTime CreatedAt { get; set; }
         public DateTime? UpdatedAt { get; set; }
         public DateTime? DeletedAt { get; set; }
        */
    }
}
