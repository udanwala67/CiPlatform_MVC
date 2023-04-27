using CiPlatform.Entitites.Data;
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
            return View();
        }

    

    }
}
