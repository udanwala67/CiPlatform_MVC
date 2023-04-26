using Microsoft.AspNetCore.Mvc;
using CiPlatform.Entitites.ViewModels;
using CiPlatform.Repository.Interface;
using CiPlatform.Entitites.Data;
using CiPlatform.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using CiPlatform.Entitites.Models;
using Microsoft.AspNetCore.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.Extensions.Hosting.Internal;

namespace CiPlatform.Controllers
{
    public class StoryController : Controller
    {
        private readonly IStoryRepository _storyRepository;
        private readonly CiContext _ciContext;
        private readonly EmailServices _emailServices;


        public StoryController(IStoryRepository storyRepository, CiContext ciContext)
        {
            _ciContext = ciContext;
            _storyRepository = storyRepository;

        }

        public IActionResult sharestory()
        {

            var shares = _storyRepository.GetStory();
            string email = HttpContext.Session.GetString("Email");
            long user = _ciContext.Users.Where(u => u.Email == email).Select(m => m.UserId).FirstOrDefault();
            ViewBag.uid = user;

            return View(shares);
        }


        [HttpPost]
        public IActionResult sharestory(StoryView storyView)
         {
            var stories = _storyRepository.GetStory();
            string Email = HttpContext.Session.GetString("Email");
            long userid = _ciContext.Users.Where(u => u.Email == Email).Select(m => m.UserId).FirstOrDefault();
            _storyRepository.PushStory(userid ,storyView );


            return View(stories);

        }


        public IActionResult storydetails()
        {
            var sunglass = _storyRepository.GetStory();
            string email = HttpContext.Session.GetString("Email");
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            ViewBag.uid = (int)user.UserId;
            return View(sunglass);
        }


        /*----------------------------------------------storydetails and Story_Details aboth are different pages so take care--------------------------------------*/


        public IActionResult Story_Details(int user, int missionId)
        {
            ViewBag.uid = (int)user.CompareTo(missionId);
            ViewBag.missionid = missionId;
            var sunglass1 = _storyRepository.GetStory();
            return View(sunglass1);
        }


       public IActionResult Timesheet()
        {
            string email = HttpContext.Session.GetString("Email");
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            ViewBag.uid= (int)user.UserId;
            var timeMission = _storyRepository.GetAllMissions(ViewBag.uid);
            return View(timeMission);

        }
        public IActionResult AddTimeMission(VolunteeringTimesheetView model)
        {
            string email = HttpContext.Session.GetString("Email");
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            int uid = (int)user.UserId;
            _storyRepository.AddTimeTimesheet(model, uid);
            var timeMission = _storyRepository.GetAllMissions(uid);
            return PartialView("_TimeMission", timeMission);
        }

     /*   public IActionResult VolunteerTimesheet()
        {
            int uid = (int)HttpContext.Session.GetInt32("UserId");
            ViewBag.uid = uid;
            var timeMission = _storyRepository.GetAllMissions(uid);
            return View(timeMission);
        }*/

       
        public IActionResult EditTimesheet(int tid)
        {
            var timesheet = _storyRepository.GetTimesheetById(tid);
            return Json(timesheet);
        }

        [HttpPost]
        public IActionResult AddGoalMission(VolunteeringTimesheetView model)
        {
            string email = HttpContext.Session.GetString("Email");
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            int uid = (int)user.UserId;
            _storyRepository.AddGoalTimesheet(model, uid);
            var goalMission = _storyRepository.GetAllMissions(uid);
            return PartialView("_GoalMission", goalMission);
        }
        public IActionResult DeleteTimesheet(int tid)
        {
            _storyRepository.deleteTimesheet(tid);
            return RedirectToAction("Timesheet", "Story");
        }

        public IActionResult getDateRange(int missionId)
        {
            var data = _storyRepository.getRange(missionId);
            return Json(data);
        }

    }

}


