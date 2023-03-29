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
      
        
        public StoryController(IStoryRepository storyRepository , CiContext ciContext)
        {
            _ciContext = ciContext;
            _storyRepository =  storyRepository;
     
        }
        public IActionResult sharestory()
        {

            var shares = _storyRepository.GetStory();
            string email = HttpContext.Session.GetString("Email");
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            ViewBag.uid = (int)user.UserId;
            return View(shares);
        }
        [HttpPost]
        public IActionResult sharestory(int mid,string storytitle, string sdate, string url)
        {
            
            string email = HttpContext.Session.GetString("Email");
            long userid = _ciContext.Users.Where(u => u.Email == email).Select(m=>m.UserId).FirstOrDefault();



            var story = new Story();
            story.UserId = userid;
            story.MissionId = mid;
            story.Title = storytitle;
            /* storymodel.PublishedAt = DateTime.Parse(sdate);*/
            story.Status = "PUBLISHED";
            story.CreatedAt = DateTime.Now;
            _ciContext.Stories.Add(story);
            _ciContext.SaveChanges();

            return RedirectToAction("sharestory");
        }


        public IActionResult storydetails()
        {
            var sunglass = _storyRepository.GetStory();
            string email = HttpContext.Session.GetString("Email");
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            ViewBag.uid = (int)user.UserId;
            return View(sunglass);
        }
    }
}
