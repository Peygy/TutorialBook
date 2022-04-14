using Microsoft.AspNetCore.Mvc;
using MainApp.Services;

namespace MainApp.Controllers
{
    public class EntryController : Controller
    {
        // Methods responsible for organizing user and staff registrations and logins
        // User registration
        [HttpGet]
        [Route("/reg")]
        public IActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        [Route("/reg")]
        public async Task<IActionResult> UserRegistration(string userLogin, string userPassword)
        {
            AuthService DbController = new AuthService();

            if (ModelState.IsValid)
            {
                if (userPassword.Length < 7)
                {
                    ModelState.AddModelError("Password", "Пароль слишком короткий");
                    return View();
                }

                if (DbController.AvailabilityCheck(userLogin))
                {
                    await DbController.AddUserAsync(userLogin, userPassword, HttpContext);
                    return Redirect("/study");
                }
                else
                {
                    ModelState.AddModelError("Login", "Пользователь с таким логином уже существует");
                    return View();
                }
            }

            return View();
        }


        // User entry (login)
        [HttpGet]
        [Route("/login")]
        public IActionResult UserIdentification()
        {
            return View();
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> UserIdentification(string userLogin, string userPassword, bool remember)
        {
            AuthService DbController = new AuthService();

            if(ModelState.IsValid)
            {
                if (await DbController.UserAuthenticationAsync(userLogin, userPassword))
                {
                    await DbController.UserAuthorizationAsync(userLogin, remember, HttpContext);
                    return Redirect("/studypage");
                }
                else
                {
                    ModelState.AddModelError("", "Логин или пароль неверны!");
                    return View();
                }
            }

            return View();
        }


        // Logout from website
        [Route("logout")]
        public async Task<IActionResult> UserLogout()
        {
            CookieService cookieService = new CookieService();
            await cookieService.LogoutAsync(HttpContext);
            return Redirect("/welcomepage");
        }




        // Admin entry (login)
        [HttpGet]
        [Route("admlog")]
        public IActionResult AdmIdentification()
        {
            return View();
        }

        [HttpPost]
        [Route("admlog")]
        public async Task<IActionResult> AdmIdentification(string admLogin, string admPassword)
        {
            AuthService DbController = new AuthService();

            if (ModelState.IsValid)
            {
                await DbController.AdmAuthenticationAsync(admLogin, admPassword, HttpContext);
                return Redirect("adm/control");
            }

            return View();
        }




        // Editor entry (login)
        [HttpGet]
        [Route("edlog")]
        public IActionResult EdIdentification()
        {
            return View();
        }

        [HttpPost]
        [Route("edlog")]
        public async Task<IActionResult> EdIdentification(string edLogin, string edPassword)
        {
            AuthService DbController = new AuthService();

            if (ModelState.IsValid)
            {
                await DbController.EdAuthenticationAsync(edLogin, edPassword, HttpContext);
                return Redirect("ed/control");
            }

            return View();
        }
    }
}
