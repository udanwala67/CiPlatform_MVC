using CiPlatform.Entitites.Data;
using CiPlatform.Entitites.Models;
using CiPlatform.Entitites.ViewModels;
using CiPlatform.Models;
using CiPlatform.Repository.Interface;
using CiPlatform.Repository.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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


        public IActionResult login(string? returnUrl)
        {
            var userEmail = HttpContext.Session.GetString("Email");
            if (userEmail != null)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("platformlandingpage", "MissionCard");
                }
            }
            return View();
        }


        [HttpPost]

        public IActionResult Login(LoginView loginView, string? returnUrl)
        {
            
            var data = _CiRepository.GetUserEmail(loginView.Email);

            if (data != null)
            {
                bool isValid = (data.Email == loginView.Email && data.Password == loginView.Password);

                if (isValid)
                {
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, data.FirstName));
                    if (data.Avatar != null)
                    {
                        claims.Add(new Claim("Avatar", data.Avatar));
                        claims.Add(new Claim("FullName", data.FirstName + " " + data.LastName));
                    }
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    HttpContext.Session.SetString("Email", loginView.Email);
                    HttpContext.Session.SetString("uid", (data.UserId.ToString()));

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    if (data.Role == "User")
                    {
                        
                        return RedirectToAction("platformlandingpage", "MissionCard");
                    }
                    else if (data.Role == "Admin")
                    {
                        return RedirectToAction("user", "Admin");
                    }
                }
            }
            // The email or password is not valid, so return an error response
            TempData["Error"] = "Invalid Email or Password!";
            return View(loginView);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("uid");
            return RedirectToAction("login");
        }

        public IActionResult forgotpassword()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Forgotpassword(ForgotPasswordView forgotView, PasswordReset passwordReset)

        {
            if (!ModelState.IsValid)
            {
                return View(forgotView);
            }
            else
            {
                var existingUser = _CiRepository.GetUserEmail(forgotView.Email);
                if (existingUser == null)
                {
                    ModelState.AddModelError("Email", "Email does not exist. Please enter valid email.");
                    return View();
                }
                string email = forgotView.Email;
                string token = (string)GenerateToken();

                _CiRepository.SaveToken(email, token);

                var resetLink = Url.Action("resetpassword", "Home", new { token = token, email = email }, "https");
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
                TempData["success"] = "Email sent successfully.";
                return View(forgotView);
            }
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

                return View();
            }
            else
            {
                var existingUser = _CiRepository.GetUserEmail(user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already exist. Please enter another email.");
                    return View();
                }

                _CiRepository.RegisterUser(user);
                /*_ciContext.Users.Add(user);*/
                return RedirectToAction("login", "Home");

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
        public IActionResult resetpassword(ResetPasswordView resetView)
        {
            var token = HttpContext.Session.GetString("token_session");
            string email = resetView.Email;
            var passreset = new PasswordReset();
            var user = _ciContext.Users.Where(x => x.Email == resetView.Email).FirstOrDefault();
            if (ModelState.IsValid)
            {
                user.Password = resetView.Password;
                user.UpdatedAt = DateTime.Now;
                _ciContext.Users.Update(user);
                _ciContext.SaveChanges();
                TempData["success"] = "Password changed successfully";
                return RedirectToAction("login");
            }
            else
            {
                return View();
            }
        }


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
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            var model = _userDetailsRepository.GetUserProfile((int)user.UserId);
            ViewBag.uid = (int)user.UserId;
            ViewBag.FullName = user.FirstName + " " + user.LastName;

            return View(model);

        }

        [HttpPost]
        public IActionResult UserEdit(string fname, string lname, string employeeid, string title, string department, string profiletext, string volunteertext, int country, int city, string linkedinurl, string hiddentext)
        {
            var identity = User.Identity as ClaimsIdentity;
            /*string name = identity.FindFirst(ClaimTypes.NameIdentifier).Value;*/
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            ViewBag.uid = (int)user.UserId;
            _userDetailsRepository.SaveAllDetails((int)user.UserId, fname, lname, employeeid, title, department, profiletext, volunteertext, country, city, linkedinurl, hiddentext);
            var model = _userDetailsRepository.GetUserProfile((int)user.UserId);
            
            if (identity != null)
            {
                var existingClaim = identity.FindFirst("FullName");

                if (existingClaim != null)
                {
                    identity.RemoveClaim(existingClaim);
                }

                identity.AddClaim(new Claim("FullName", fname+" "+lname ));
                var authenticationProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };
                 HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authenticationProperties);
            }

            return View(model);
        }
        [HttpPost]

        public async Task<IActionResult> changeAvatar(UserProfileView model)
        {
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }
            var user = _ciContext.Users.FirstOrDefault(u => u.Email == email);
            int uid = (int)user.UserId;
            var userp = _ciContext.Users.Where(x => x.UserId == uid).FirstOrDefault();
            string filename = Path.GetFileName(model.AvatarImage.FileName);
            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\UploadedFiles", filename);
            var filestream = new FileStream(uploadPath, FileMode.Create);
            model.AvatarImage.CopyTo(filestream);
            string dbfilepath = "/UploadedFiles/" + filename;
            userp.Avatar = dbfilepath;
            _ciContext.Users.Update(userp);
            _ciContext.SaveChanges();

            var identity = User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var existingClaim = identity.FindFirst("Avatar");

                if (existingClaim != null)
                {
                    identity.RemoveClaim(existingClaim);
                }

                identity.AddClaim(new Claim("Avatar", userp.Avatar));
                var authenticationProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authenticationProperties);
            }
            return RedirectToAction("UserEdit", "Home");
        }


        public IActionResult GetUserProfile()
        {
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }
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
        public IActionResult ChangePass(string oldpass, string newpass, string cnewpass)
        {

            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }
            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            _userDetailsRepository.updatePass((int)user.UserId, oldpass, newpass, cnewpass);
            return RedirectToAction("Login", "Home");



            /* // there was an error creating the user, display error messages
             return RedirectToAction("UserEdit");*/


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

        public IActionResult PolicyPage()
        {
            var email = HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            var user = _ciContext.Users.Where(u => u.Email == email).FirstOrDefault();
            var model = _userDetailsRepository.GetUserProfile((int)user.UserId);
            ViewBag.uid = (int)user.UserId;
            return View(model);

        }
    }
}