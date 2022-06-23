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
        private AuthService authService;
        private CookieService cookieService;

        public EntryController(UserContext _db , ILogger<AuthService> _logger)
        {
            data = _db;
            logger = _logger;
        }



        // Account registration
        [HttpGet]
        public IActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserRegistration(CreateUserModel newUser)
        {
            authService = new AuthService(data, logger, HttpContext);

            if (ModelState.IsValid)
            {
                if (authService.AvailabilityCheck(newUser.Login))
                {
                    await authService.AddUserAsync(newUser.Login, newUser.Password);
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



        // Account login
        [HttpGet]
        public IActionResult UserLogin()
        {
            cookieService = new CookieService(HttpContext);

            if (cookieService.GetUserInfo().Login != null)
            {
                return RedirectToAction("Study","Page");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(User user, bool remember)
        {
            authService = new AuthService(data, logger, HttpContext);

            if (ModelState.IsValid)
            {
                if (await authService.UserAuthenticationAsync(user.Login, user.Password))
                {
                    await authService.UserAuthorizationAsync(user.Login, remember);
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
            cookieService = new CookieService(HttpContext);
            await cookieService.LogoutAsync();
            return RedirectToAction("Welcome","Page");
        }




        [HttpGet]
        public IActionResult CrewLogin()
        {
            cookieService = new CookieService(HttpContext);

            switch (cookieService.GetUserInfo().Role) 
            {
                case "admin" or "editor": return RedirectToAction("ViewParts","Part");
                case "user": return RedirectToAction("Study", "Page");
                case "null": return View();
            }

            return null;
        }

        [HttpPost]
        public async Task<IActionResult> CrewLogin(Admin admin)
        {
            authService = new AuthService(data, logger, HttpContext);

            if (ModelState.IsValid)
            {
                if (await authService.AdmAuthenticationAsync(admin.Login, admin.Password) 
                || await authService.EdAuthenticationAsync(admin.Login, admin.Password))
                {
                    return RedirectToAction("ViewParts","Part");
                }

                ViewBag.Error = "Логин или пароль неверны!";
                return View(admin);
            }

            return View(admin);
        }
    }
}
