using CiPlatform.Entitites.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Entitites.ViewModels
{
    public class AdminView
    {
        public User user { get; set; }
        public List<User> users { get; set; }
        public List<MissionSkill> missionSkills { get; set; }
        public List<CmsPage> cmspages { get; set; }
        public List<Mission> missions { get; set; }   
        public List<MissionApplication> missionApplications { get; set; }
        public List<MissionTheme> missionThemes { get; set; }
        public List<Country> country { get; set; }
        public List<City> city { get; set; }
        public List<Skill> skills { get; set; }
        public List<Story> stories { get; set; }
        public List<Banner> banners { get; set; }
        public long UserId { get; set; }
        public IFormFile AvatarImage { get; set; }
    }

}
