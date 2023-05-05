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
        public List<StoryMedium> storyMedia { get; set; }
        public List<IFormFile> UploadedFiles { get; set; }
        public List<MissionApplication> missionApplication { get; set; }
        public List<Timesheet> timesheet { get; set; }
        public long StoryId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? url { get; set; }
        public DateTime? date { get; set; }
        public int mid { get; set; }
        public string? Status { get; set; } = string.Empty;




        //public Mission missions { get; set; }
        /* public List<Models.User> users { get;}*/
        /* public IFormFile StoryMedia { get; set; }
         public List<StoryMedium> storyMedia { get; set; }*/
    }
}
