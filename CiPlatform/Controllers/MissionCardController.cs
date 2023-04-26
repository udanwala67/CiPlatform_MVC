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
        private readonly EmailServices _emailServices;
        public MissionCardController(ICiRepository ciRepository, CiContext ciContext, EmailServices emailServices)
        {
            _ciContext = ciContext;
            _CiRepository = ciRepository;
            _emailServices = emailServices;
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
                /*var Availability = _ciContext.Missions.FirstOrDefault(u => u.Availability == item.Availability);*/
            }
            string email = HttpContext.Session.GetString("Email");
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            ViewBag.uid = (int)user.UserId;
            ViewBag.mission = mission;
            return View(ms);
        }
        public JsonResult getCities(string country)
        {
            var con = _ciContext.Countries.Where(c => c.Name == country).FirstOrDefault();
            var city = _ciContext.Cities.Where(c => c.CountryId == con.CountryId);
            return Json(city);
        }
        public IActionResult VolunteeringMission(int missionid, string theme)
        {
            ViewBag.missionid = missionid;
            string email = HttpContext.Session.GetString("Email");

            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            ViewBag.theme = theme;
            
            

            if (user == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.uid = (int)user.UserId;
                var mission = _CiRepository.GetMission();
                
                return View(mission);
            }
        }

        public IActionResult applyvol(int userId,int missionId)
        {
            _CiRepository.applyvol(userId, missionId);
            return RedirectToAction("VolunteeringMission", "MissionCard" , new { missionId = missionId });
        }

        public IActionResult AddtoFav(int mid)
        {
            string email = HttpContext.Session.GetString("Email");
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            int uid = (int)user.UserId;
            _CiRepository.AddToFavourite(uid, mid);
            ViewBag.mid = mid;
            return RedirectToAction("platformlandingpage");
        }

        public IActionResult coworker(int missionId, string email)
        {
            UriBuilder builder = new UriBuilder();
            builder.Scheme = "https";
            builder.Host = "localhost";
            builder.Port = 7148;
            builder.Path = "MissionCard/VolunteeringMission";
            builder.Query = "?missionid=" + missionId;

            var resetLink = builder.ToString();

            _emailServices.SendEmailAsync(email, "Recommend Coworker", resetLink);

            return RedirectToAction("VolunteeringMission", new { missionId = missionId });
        }



        [HttpPost]
        public IActionResult AddToComment(int missionId, int userId, string comment)
        {
            Comment com = new Comment();
            com.UserId = userId;
            com.MissionId = missionId;
            com.Comment1 = comment;
            com.CreatedAt = DateTime.Now;
            _ciContext.Comments.Add(com);
            _ciContext.SaveChanges();

            return RedirectToAction("VolunteeringMission");
        }

        
    }
}
