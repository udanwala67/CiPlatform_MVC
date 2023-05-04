using CiPlatform.Entitites.Data;
using CiPlatform.Entitites.Models;
using CiPlatform.Entitites.ViewModels;
using CiPlatform.Repository.Interface;
using CiPlatform.Repository.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CiPlatform.Controllers
{
    public class AdminController : Controller
    {
        private readonly CiContext _ciContext;
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository, CiContext ciContext)
        {
            _ciContext = ciContext;
            _adminRepository = adminRepository;
        }

        public IActionResult user()
        {
            var shares = _adminRepository.GetData();
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }
            long user = _ciContext.Users.Where(u => u.Email == email).Select(m => m.UserId).FirstOrDefault();
            ViewBag.uid = user;

            return View(shares);
        }
        [HttpPost]

        public IActionResult user(AdminView model)
        {
            _adminRepository.AddUser(model);
            return RedirectToAction("user");
        }

        public IActionResult GetUserData(int uid)
        {
            var user = _adminRepository.GetUser(uid);
            return Json(user);

        }

        public IActionResult DeleteUser(int uid)
        {
            _adminRepository.deleteUser(uid);
            var model = _adminRepository.GetData();
            /* return RedirectToAction("user", "Admin");*/
            return PartialView("_Admin");
        }
        public JsonResult getCities(string selectedcountry)
        {
            var con = _ciContext.Countries.Where(c => c.Name == selectedcountry).FirstOrDefault();
            var city = _ciContext.Cities.Where(c => c.CountryId == con.CountryId);
            return Json(city);
        }
        public IActionResult MissionPage()
        {
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            ViewBag.uid = (int)user.UserId;
            var model = _adminRepository.GetData();
            return View(model);
        }

        [HttpPost]

        public IActionResult MissionPage(AdminView model)
        {
            _adminRepository.AddUser(model);
            return RedirectToAction("MissionPage");
        }

        public IActionResult CmsPage()
        {
            return View();
        }



        public IActionResult MissionTheme()
        {
            return View();
        }

        public IActionResult MissionSkills()
        {
            return View();
        }

        public IActionResult MissionApplication()
        {
            return View();
        }
        [HttpPost]
        public IActionResult MissionApplication(AdminView model)
        {
            _adminRepository.GetData();
            return View();
        }

        public IActionResult StoryPage()
        {
            return View();
        }
    }
}