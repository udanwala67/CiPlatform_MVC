using CiPlatform.Entitites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Entitites.ViewModels
{
    public class UserProfileView
    {
        public List<User> users { get; set; }
        public List<Skill> skills { get; set; }
        public List<UserSkill> userSkills { get; set; }
        public List<Country> country { get; set; }
        public List<City> city { get; set; }
        public string? Avatar { get; set; }

    }
}