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

        public AdminController(IAdminRepository adminRepository,CiContext ciContext)
        {
            _ciContext = ciContext;
            _adminRepository = adminRepository;
        }

        public IActionResult user()
        {
            string email = HttpContext.Session.GetString("Email");
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            ViewBag.uid = (int)user.UserId;
            var model = _adminRepository.GetData();
            return View(model);
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
            string email = HttpContext.Session.GetString("Email");
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

    }
}