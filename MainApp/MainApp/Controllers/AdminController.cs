using Microsoft.AspNetCore.Mvc;
using MainApp.Services;
using Microsoft.AspNetCore.Authorization;
using MainApp.Models;

namespace MainApp.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        [Route("/login/adm")]   
        [Authorize(Roles = "admin, editor")]
        public IActionResult Identification()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Identification(Admin adm)
        {
            AuthService DbController = new AuthService();

            if (!ModelState.IsValid)
            {
                return View(adm);
            }

            if (await DbController.AdmAuthenticationAsync(adm))
            {
                await DbController.AdmAuthorizationAsync(adm);
            }
            else
            {
                ViewBag.Message = "Логин или пароль неверны.";
                return Json(false);
            }

            return Redirect("/admpanel");
        }


        [Route("/admpanel")] // Страничка управления админа
        [Authorize(Roles = "admin, editor")]
        public IActionResult AdminPage()
        {
            return View();
        }

        [Route("/admpanel/addpost")]
        [Authorize(Roles = "admin, editor")]
        public IActionResult AddPost(Post post)
        {
            return View();
        }

        [Route("/admpanel/users")]
        [Authorize(Roles = "admin")]
        public IActionResult ViewUsers(Post post)
        {
            //ViewBag.Users = 
            return View();
        }
    }
}
