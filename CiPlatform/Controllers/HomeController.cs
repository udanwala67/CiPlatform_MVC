using CiPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CiPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ILogger<HomeController> Logger => _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult login()
        {
            return View();
        }
        public IActionResult forgotpassword()
        {
            return View();
        }
        public IActionResult registration()
        {
            return View();
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