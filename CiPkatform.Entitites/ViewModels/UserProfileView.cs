using CiPlatform.Entitites.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Entitites.ViewModels
{
    public class UserProfileView
    {
        public List<User> user = new List<User>();
     
        public List<Skill> skills { get; set; }
        public List<UserSkill> userSkills { get; set; }
        public List<Country> country { get; set; }
        public List<City> city { get; set; }
        public string? Avatar { get; set; }
        [Display(Name = "Profile Photo")]
        public IFormFile AvatarImage { get; set; }

       /* [Required]
       *//*[DataType(DataType.Password)]*//*
        public string password { get; set; }

        [Required]
        *//*[DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]*//*
        public string ConfirmPassword { get; set; }*/
        
    }

}
