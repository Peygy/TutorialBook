using Microsoft.AspNetCore.Mvc;
using MainApp.Services;
using Microsoft.AspNetCore.Authorization;
using MainApp.Models;

namespace MainApp.Controllers
{
    public class EntryController : Controller
    {
        // User entry methods
        // Registration
        [HttpGet]
        [Route("reg")]
        public IActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        [Route("reg")]
        public async Task<IActionResult> UserRegistration(string userLogin, string userPassword)
        {
            AuthService DbController = new AuthService();

            if (userPassword.Length < 7)
            {
                ModelState.AddModelError("Password", "Пароль слишком короткий");
                return View();
            }


            if (DbController.AvailabilityCheck(userLogin))
            {
                await DbController.AddUserAsync(userLogin, userPassword, HttpContext);
            }
            else
            {
                ModelState.AddModelError("Login", "Пользователь с таким логином уже существует");
                return View();
            }

            return Redirect("/study");
        }


        // Login
        [HttpGet]
        [Route("login")]
        public IActionResult UserIdentification()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> UserIdentification(string userLogin, string userPassword, bool remember)
        {
            AuthService DbController = new AuthService();

            /*if (user.Login == null || user.Password == null)
            {
                if (user.Login == null && user.Password == null)
                {
                    ViewData["LoginEmpty"] = "Поле ввода логина не может быть пустым.";
                    ViewData["PasswordEmpty"] = "Поле ввода пароля не может быть пустым.";
                    return View();
                }
                else if (user.Login == null)
                {
                    ViewData["LoginEmpty"] = "Поле ввода логина не может быть пустым.";
                    return View();
                }
                else if (user.Password == null)
                {
                    ViewData["PasswordEmpty"] = "Поле ввода пароля не может быть пустым.";
                    return View();
                }
            }*/

            if (await DbController.UserAuthenticationAsync(userLogin, userPassword))
            {
                await DbController.UserAuthorizationAsync(userLogin, remember, HttpContext);
            }
            else
            {
                ViewBag.Message = "Логин или пароль неверны.";
                return View();
            }

            return Redirect("/study");
        }


        // Logout
        [Route("logout")]
        public async Task<IActionResult> UserLogout()
        {
            CookieService cookieService = new CookieService();
            await cookieService.LogoutAsync(HttpContext);
            return Redirect("/main");
        }





        // Admin entry methods
        // Login
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

            if (!ModelState.IsValid)
            {
                return View();
            }

            await DbController.AdmAuthenticationAsync(admLogin, admPassword, HttpContext);
            return Redirect("adm/control");
        }





        // Editor entry methods
        // Login
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

            if (!ModelState.IsValid)
            {
                return View();
            }

            await DbController.EdAuthenticationAsync(edLogin, edPassword, HttpContext);
            return Redirect("ed/control");
        }
    }
}
