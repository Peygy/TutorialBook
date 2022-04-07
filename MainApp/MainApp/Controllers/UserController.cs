using Microsoft.AspNetCore.Mvc;
using MainApp.Services;
using MainApp.Models;

namespace MainApp.Controllers
{
    public class UserController : Controller
    {  
        [HttpGet]
        [Route("/register")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(User user)
        {
            AuthService DbController = new AuthService();

            if (user.Password.Length < 7)
            {
                ModelState.AddModelError("Password", "Пароль слишком короткий");
                return View(user);
            }          


            if (await DbController.AvailabilityCheckAsync(user.Login))
            {
                await DbController.AddUserAsync(user, HttpContext);
            }
            else
            {
                ModelState.AddModelError("Login", "Пользователь с таким логином уже существует");
                return View(user);
            }

            return Redirect("/study");
        }


        [HttpGet]
        [Route("/login")]
        public IActionResult Identification()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Identification(User user, bool remember)
        {
            AuthService DbController = new AuthService();

            if (user.Login == null || user.Password == null)
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
            }

            if (await DbController.UserAuthenticationAsync(user))
            {
                await DbController.UserAuthorizationAsync(user, remember, HttpContext);
            }
            else
            {
                ViewBag.Message = "Логин или пароль неверны.";
                return View();
            }

            return Redirect("/study");
        }

        [Route("/logout")]
        public async Task<IActionResult> Logout()
        {
            CookieService cookieService = new CookieService();
            await cookieService.Logout(HttpContext);
            return Redirect("/main");
        }
    }
}
