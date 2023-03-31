﻿using CiPlatform.Entitites.Models;
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
        public int mid { get; set; }
        public List<MissionApplication> missionApplication { get; set; }

        //public Mission missions { get; set; }
       // public MissionApplication missionApplication { get; set; }


        /* public List<Models.User> users { get;}*/
       /* public IFormFile StoryMedia { get; set; }
        public List<StoryMedium> storyMedia { get; set; }*/
    }
}
