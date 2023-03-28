using CiPlatform.Entitites.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Entitites.ViewModels
{
    public class StoryView
    {
        public List<User> user { get; set; }
        public List<Mission> mission { get; set; }
        public List<Story> stories { get; set; }
        public List<MissionTheme> missionTheme { get; set; }
        public IFormFile StoryMedia { get; set; }
        public List<StoryMedium> storyMedia { get; set; }
    }
}
