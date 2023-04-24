using CiPlatform.Entitites.Data;
using CiPlatform.Entitites.Models;
using CiPlatform.Entitites.ViewModels;
using CiPlatform.Models;
using CiPlatform.Repository.Interface;
using CiPlatform.Repository.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace CiPlatform.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly ICiRepository _CiRepository;
        private readonly EmailServices _emailServices;
        private readonly CiContext _ciContext;
        private readonly IUserDetailsRepository _userDetailsRepository;
        public ILogger<HomeController> Logger => _logger;

        public HomeController(ILogger<HomeController> logger, ICiRepository ciRepository, EmailServices emailServices, IUserDetailsRepository userDetailsRepository, CiContext ciContext)
        {
            _logger = logger;
            _CiRepository = ciRepository;
            _emailServices = emailServices;
            _userDetailsRepository = userDetailsRepository;
            _ciContext = ciContext;
            
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

            if (cuser != null && cuser.Password.Equals(user.Password) && ModelState.IsValid)
            {
                HttpContext.Session.SetString("Email", cuser.Email);
                return RedirectToAction("platformlandingpage","MissionCard");
            }

            else
            {
                ModelState.AddModelError("Password", "Invalid email or password.");
                return View();
            }
        }


        public IActionResult forgotpassword()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Forgotpassword(ForgotPasswordView forgotView, PasswordReset passwordReset)

        {
            string email = forgotView.Email;
            string token = (string)GenerateToken();
            UriBuilder builder = new UriBuilder();
            builder.Scheme = "https";
            builder.Host = "localhost";
            builder.Port = 7148;
            builder.Path = "Home/resetpassword";
            builder.Query = "token=" + token + "&email=" + email;
            _CiRepository.SaveToken(email, token);

            var resetLink = builder.ToString();
            _emailServices.SendEmailAsync(email, "Reset Password Link", resetLink);

            /*("token_session", token.ToString());*/

            HttpContext.Session.SetString("token_session", token.ToString());
            var view = new PasswordReset()
            {
                Email = email,
                Token = token,
                CreatedAt = DateTime.Now,
            };

            _ciContext.PasswordResets.Add(view);
            _ciContext.SaveChanges();
            return RedirectToAction("login");
        }
        public IActionResult registration()
        {
            return View();
        }


        [HttpPost]

        public IActionResult Registration(User user)
        {
            if (ModelState.IsValid)
            {

                _CiRepository.RegisterUser(user);
                return RedirectToAction("login","Home");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult resetpassword(string token, string email, PasswordReset passwordReset)
        {
            if (token != null && token == passwordReset.Token)
            {
                ResetPasswordView resetView = new ResetPasswordView();
                resetView.Email = email;
                return View(resetView);
            }
            return View();
        }

        [HttpPost]
        public IActionResult resetpassword(ResetPasswordView resetView, IFormCollection form)
        {
            /*string token = HttpContext.Session.GetString("token_session");*/
            string email = resetView.Email;
            var passreset = new PasswordReset();
            var user = _ciContext.Users.Where(x => x.Email == resetView.Email).FirstOrDefault();
            /*if (form["confirmpass"] == form["password"])
           {*/

            user.Password = resetView.Password;
            user.UpdatedAt = DateTime.Now;
            _ciContext.Users.Update(user);
            _ciContext.SaveChanges();
            return RedirectToAction("login");
        }
        /* else{
         return View()
         }*/

        private object GenerateToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var token = new String(Enumerable.Repeat(chars, 32).Select(s => s[random.Next(s.Length)]).ToArray());
            return token;
        }




        public IActionResult listview()
        {
            return View();
        }
        public IActionResult missionnotfound()
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
     
        public IActionResult planding()
        {
            return View();
        }

        public IActionResult UserEdit()
        {
            var identity = User.Identity as ClaimsIdentity;
            /*string name = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewBag.Name = name;*/
            string email = HttpContext.Session.GetString("Email");
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            var model = _userDetailsRepository.GetUserProfile((int)user.UserId);
            return View(model);

        }

        [HttpPost]
        public IActionResult UserEdit(string fname, string lname, string employeeid, string title, string department, string profiletext, string volunteertext, int country, int city, string linkedinurl, string hiddentext)
        {
            var identity = User.Identity as ClaimsIdentity;
            /*string name = identity.FindFirst(ClaimTypes.NameIdentifier).Value;*/
            string email = HttpContext.Session.GetString("Email");
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
           
            _userDetailsRepository.SaveAllDetails((int)user.UserId, fname, lname, employeeid, title, department, profiletext, volunteertext, country, city, linkedinurl, hiddentext);
            var model = _userDetailsRepository.GetUserProfile((int)user.UserId);   
           
            return View(model);
        }

        public JsonResult GetUserProfile()
        {
            string email = HttpContext.Session.GetString("Email");
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            var profile = _userDetailsRepository.GetUserById((int)user.UserId);
            if (profile != null)
            {
                return Json(profile);
            }
            else
            {
                return Json(false);
            }
        }

        public JsonResult getCities(int country)
        {
            var city = _ciContext.Cities.Where(c => c.CountryId == country);
            return Json(city);
        }

        [HttpPost]
        public IActionResult ChangePass(string oldpass,string newpass,string cnewpass)
        {
            string email = HttpContext.Session.GetString("Email");
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            _userDetailsRepository.updatePass((int)user.UserId, oldpass, newpass, cnewpass);
 
            return RedirectToAction("login","home");
        }

        public IActionResult getProfileImage(int uid)
        {
            string email = HttpContext.Session.GetString("Email");
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            _userDetailsRepository.GetUserPhotoById((int)user.UserId);



            return View();
        }
        [HttpPost]

        public IActionResult getProfileImage(UserProfileView userProfileView)
        {
            return View(userProfileView);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
