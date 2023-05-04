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
     
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        
        [Required]
        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;
       
    }
}
