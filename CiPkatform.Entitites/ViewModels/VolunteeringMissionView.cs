using CiPlatform.Entitites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiPlatform.Entitites.ViewModels
{
    public class VolunteeringMissionView
    {
        public List<Mission> mission { get; set; }
        public List<City> city { get; set; }
        public List<Country> country { get; set; }
        public List<GoalMission> goalMission { get; set; }
        public List<FavoriteMission> favoriteMission { get; set; } 
        public List<MissionRating> missionRating { get; set; }
        public List<MissionTheme> missionTheme { get; set; }    
        public List<User>user { get; set; }
        public List<Comment> comments { get; set; }
        public List<MissionApplication> application { get; set; }   
        public List<MissionDocument> document { get; set; }

    }
}
