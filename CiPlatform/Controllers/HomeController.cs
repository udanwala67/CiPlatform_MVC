using CiPlatform.Entitites.Models;
using CiPlatform.Entitites.ViewModels;
using CiPlatform.Models;
using CiPlatform.Repository.Interface;
using CiPlatform.Repository.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CiPlatform.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly ICiRepository _CiRepository;
      /*  public ILogger<HomeController> Logger => _logger;*/

        public HomeController(ILogger<HomeController> logger, ICiRepository ciRepository)
        {
            _logger = logger;
            _CiRepository = ciRepository;
        }


        public IActionResult Index()
        {
          return View();

        }


        public IActionResult login()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Login(User user)
        {
            var cuser = _CiRepository.GetUserEmail(user.Email);
            if (cuser != null && cuser.Password.Equals(user.Password) && (!ModelState.IsValid) )
            {
                return RedirectToAction("platformlandingpage");
            }
            else
            {
                return View();
            }
        }
        public IActionResult forgotpassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Forgotpassword(ForgotPasswordView forgotView ,PasswordReset passwordReset)
        {
            string email = forgotView.Email;
            string token = (string);
            UriBuilder builder = new UriBuilder();
            builder.Scheme = "https";
            builder.Host = "localhost";

            return View("Index");

        }
        public IActionResult registration()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Registration(User user)
        {
            if (!ModelState.IsValid)
            {

                _CiRepository.RegisterUser(user);
                return RedirectToAction("login");
            }
            else
            {
                return View();
            }
        }
        public IActionResult resetpassword()
        {
            return View();
        }
        public IActionResult platformlandingpage()
        {
            return View();
        }

        public IActionResult listview()
        {
            return View();
        }
        public IActionResult missionnotfound()
        {
            return View();
        }
        public IActionResult sharestory()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult relatedmission()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}