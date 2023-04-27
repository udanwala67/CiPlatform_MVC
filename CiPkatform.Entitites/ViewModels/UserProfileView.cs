using CiPlatform.Entitites.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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
        public List<IFormFile> UploadedFiles { get; set; }

    }
}