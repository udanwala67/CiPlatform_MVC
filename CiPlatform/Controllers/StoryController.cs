using Microsoft.AspNetCore.Mvc;
using CiPlatform.Entitites.ViewModels;
using CiPlatform.Repository.Interface;
using CiPlatform.Entitites.Data;
using CiPlatform.Repository.Repository;
using Microsoft.EntityFrameworkCore;

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
            return View();
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
