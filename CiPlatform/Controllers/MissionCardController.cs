using CiPlatform.Entitites.Data;
using CiPlatform.Entitites.Models;
using CiPlatform.Entitites.ViewModels;
using CiPlatform.Repository.Interface;
using CiPlatform.Repository.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CiPlatform.Controllers
{
   
    public class MissionCardController : Controller
    {
        private readonly CiContext _ciContext;
        private readonly ICiRepository _CiRepository;

        public MissionCardController(ICiRepository ciRepository , CiContext ciContext)
        {
            _ciContext = ciContext;
            _CiRepository = ciRepository;
        }
        public IActionResult platformlandingpage(string searchQuery, string sortOrder, CityView cityView)
        {
            List<Mission> mission = _ciContext.Missions.ToList();
            var ms = _CiRepository.GetMission();
           
            List<Country> country = _ciContext.Countries.ToList();
           

            List<City> city = _ciContext.Cities.ToList();
            

            List<MissionTheme> themes = _ciContext.MissionThemes.ToList();

           


            if (searchQuery != null)
            {
                mission = _ciContext.Missions.Where(m => m.Title.Contains(searchQuery)).ToList();
                ViewBag.InputSearch = searchQuery;

                if (mission.Count() == 0)
                {
                    return RedirectToAction("missionnotfound", "home");
                }
  
            }

            int TotalMissions = mission.Count();
            ViewBag.TotalMissions = TotalMissions;

            switch (sortOrder)
            {
                case "Newest":
                    mission = _ciContext.Missions.OrderByDescending(m => m.StartDate).ToList();
                    break;

                case "Oldest":
                    mission = _ciContext.Missions.OrderByDescending(mission => mission.EndDate).ToList();
                    break;

                case "Theme":
                    mission = _ciContext.Missions.OrderByDescending(mission => mission.Theme).ToList();
                    break;
            }

            foreach (var item in mission)
            {
                var City = _ciContext.Cities.FirstOrDefault(u => u.CityId == item.CityId);
                var Theme = _ciContext.MissionThemes.FirstOrDefault(u => u.MissionThemeId == item.ThemeId);
                /*   var Availability = _ciContext.Missions.FirstOrDefault(u => u.Availability == item.Availability);*/
            }
            string email = HttpContext.Session.GetString("Email");
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            ViewBag.uid = (int)user.UserId;
            ViewBag.mission = mission;
            return View(ms);
        }

        public IActionResult VolunteeringMission(int missionid)
        {
            ViewBag.missionid = missionid;
            string email = HttpContext.Session.GetString("Email");

            var user = _ciContext.Users.Where(u=>u.Email == email).FirstOrDefault();

            

           var mission = _CiRepository.GetMission();

            return View(mission);
}

        public IActionResult AddtoFav(int mid)
        {
            //var obj = _cardRepository.GetAllMissions();
            string email = HttpContext.Session.GetString("Email");
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            int uid = (int)user.UserId;
            _CiRepository.AddToFavourite(uid, mid);
            ViewBag.mid = mid;
            return RedirectToAction("platformlandingpage");
        }
    }
}
