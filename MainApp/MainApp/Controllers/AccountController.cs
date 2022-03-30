using Microsoft.AspNetCore.Mvc;
using MainApp.Services.Auth;
using MainApp.Models;

namespace MainApp.Controllers
{
    public class AccountController : Controller
    {  
        [HttpGet]
        [Route("/register")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(string userLogin, string userPassword)
        {
            DataService DbController = new DataService();

            if (userLogin == null || userPassword == null)
            {
                if (userLogin == null && userPassword == null)
                {
                    ViewData["LoginEmpty"] = "Поле ввода логина не может быть пустым.";
                    ViewData["PasswordEmpty"] = "Поле ввода пароля не может быть пустым.";
                    return View();
                }
                else if (userLogin == null)
                {
                    ViewData["LoginEmpty"] = "Поле ввода логина не может быть пустым.";
                    return View();
                }
                else if (userPassword == null)
                {
                    ViewData["PasswordEmpty"] = "Поле ввода пароля не может быть пустым.";
                    return View();
                }
            }
            

            if(DbController.AvailabilityCheck(userLogin) == true)
            {
                await DbController.AddUser(userLogin, userPassword, HttpContext);
            }
            else
            {
                ViewBag.Message = "Пользователь с данным логином существует.";
                return View();
            }

            return Redirect("/main");
        }


        [HttpGet]
        [Route("/login")]
        public IActionResult Identification()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Identification(string userLogin, string userPassword, bool remember)
        {
            DataService DbController = new DataService();

            if (userLogin == null || userPassword == null)
            {
                if (userLogin == null && userPassword == null)
                {
                    ViewData["LoginEmpty"] = "Поле ввода логина не может быть пустым.";
                    ViewData["PasswordEmpty"] = "Поле ввода пароля не может быть пустым.";
                    return View();
                }
                else if (userLogin == null)
                {
                    ViewData["LoginEmpty"] = "Поле ввода логина не может быть пустым.";
                    return View();
                }
                else if (userPassword == null)
                {
                    ViewData["PasswordEmpty"] = "Поле ввода пароля не может быть пустым.";
                    return View();
                }
            }

            if (DbController.Authentication(userLogin, userPassword) == true)
            {
                await DbController.Authorization(userLogin, userPassword, remember, HttpContext);
            }
            else
            {
                ViewBag.Message = "Логин или пароль неверны.";
                return View();
            }

            return Redirect("/main");
        }

        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
