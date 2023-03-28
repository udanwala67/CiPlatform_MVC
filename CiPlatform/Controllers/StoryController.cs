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
