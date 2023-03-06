using CiPlatform.Entitites.Data;
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
        private readonly EmailServices _emailServices;
        private readonly CiContext _ciContext;
        /*  public ILogger<HomeController> Logger => _logger;*/

        public HomeController(ILogger<HomeController> logger, ICiRepository ciRepository, EmailServices emailServices, CiContext ciContext)
        {
            _logger = logger;
            _CiRepository = ciRepository;
            _emailServices = emailServices;
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

            /* ("token_session", token.ToString());*/

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
                    return RedirectToAction("login");
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
            public IActionResult Resetpassword(ResetPasswordView resetView, IFormCollection form)
            {
                string token = HttpContext.Session.GetString("token_session");
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
