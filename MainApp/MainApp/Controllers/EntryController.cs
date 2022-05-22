using Microsoft.AspNetCore.Mvc;
using MainApp.Services;
using MainApp.Models;

namespace MainApp.Controllers
{
    // Controller for managing user and staff registrations and logins
    public class EntryController : Controller
    {
        // Data context for users and crew
        private UserContext data;
        // Logger for exceptions
        private ILogger<AuthService> logger;
        public EntryController(UserContext _db , ILogger<AuthService> _logger)
        {
            data = _db;
            logger = _logger;
        }

        

        [HttpGet]
        public IActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserRegistration(CreateUserModel newUser)
        {
            AuthService DbController = new AuthService(data, logger);

            if (ModelState.IsValid)
            {
                if (DbController.AvailabilityCheck(newUser.Login))
                {
                    await DbController.AddUserAsync(newUser.Login, newUser.Password, HttpContext);
                    return RedirectToAction("Study","Page");
                }
                else
                {
                    ViewBag.Error = "Пользователь с таким логином уже существует";
                    return View(newUser);
                }
            }

            return View(newUser);
        }



        [HttpGet]
        public IActionResult UserLogin()
        {
            CookieService cookieService = new CookieService(HttpContext);

            if (cookieService.GetUserInfo().Login != null)
            {
                return RedirectToAction("Study","Page");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(User user, bool remember)
        {
            AuthService DbController = new AuthService(data, logger);

            if (ModelState.IsValid)
            {
                if (await DbController.UserAuthenticationAsync(user.Login, user.Password))
                {
                    await DbController.UserAuthorizationAsync(user.Login, remember, HttpContext);
                    return RedirectToAction("Study","Page");
                }
                else
                {
                    ViewBag.Error = "Логин или пароль неверны!";
                    return View(user);
                }
            }

            return View(user);
        }



        public async Task<IActionResult> Logout()
        {
            CookieService cookieService = new CookieService(HttpContext);
            await cookieService.LogoutAsync();
            return RedirectToAction("Welcome","Page");
        }




        [HttpGet]
        public IActionResult CrewLogin()
        {
            CookieService cookieService = new CookieService(HttpContext);

            switch (cookieService.GetUserInfo().Role) 
            {
                case "admin": return RedirectToAction("ViewParts","Part");
                case "editor": return RedirectToAction("ViewParts","Part");
                case "null": return View("~/Views/Entry/CrewLogin.cshtml");
            }

            return null;
        }

        [HttpPost]
        public async Task<IActionResult> CrewLogin(Admin admin)
        {
            AuthService DbController = new AuthService(data, logger);

            if (ModelState.IsValid)
            {
                if (await DbController.AdmAuthenticationAsync(admin.Login, admin.Password, HttpContext) 
                || await DbController.EdAuthenticationAsync(admin.Login, admin.Password, HttpContext))
                {
                    return RedirectToAction("ViewParts","Part");
                }
                ViewBag.Error = "Логин или пароль неверны!";
                return View("~/Views/Entry/CrewLogin.cshtml", admin);
            }

            return View("~/Views/Entry/CrewLogin.cshtml", admin);
        }
    }
}
