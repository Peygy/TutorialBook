using Microsoft.AspNetCore.Mvc;
using MainApp.Tests.Models.Repositories;
using MainApp.Tests.Models.Crew;

namespace MainApp.Tests.Controllers.Entry
{
    // A imitation of the EntryController from the MainApp for controller testing
    public class EntryController : Controller
    {
        // Interface with method imitations for controller working
        private readonly IEntryRepository repo;

        public EntryController(IEntryRepository _repo)
        {
            repo = _repo;
        }



        public IActionResult UserRegistration(CreateUserModel newUser)
        {
            if (ModelState.IsValid)
            {
                if (repo.AvailabilityCheck(newUser.Login))
                {
                    repo.AddUser(newUser.Login, newUser.Password);
                    return RedirectToAction("Study", "Page");
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
            if (repo.GetUserInfo().Login != null)
            {
                return RedirectToAction("Study", "Page");
            }
            return View();
        }

        [HttpPost]
        public IActionResult UserLogin(User user, bool remember)
        {
            if (ModelState.IsValid)
            {
                if (repo.UserAuthentication(user.Login, user.Password))
                {
                    repo.UserAuthorization(user.Login, remember);
                    return RedirectToAction("Study", "Page");
                }
                else
                {
                    ViewBag.Error = "Логин или пароль неверны!";
                    return View(user);
                }
            }

            return View(user);
        }



        public IActionResult Logout()
        {
            repo.Logout();
            return RedirectToAction("Welcome", "Page");
        }



        [HttpGet]
        public IActionResult CrewLogin()
        {
            switch (repo.GetUserInfo().Role)
            {
                case "admin" or "editor": return RedirectToAction("ViewParts", "Part");
                case "user": return RedirectToAction("Study", "Page");
                case "null": return BadRequest();
            }

            return null;
        }

        [HttpPost]
        public IActionResult CrewLogin(Admin admin)
        {
            if (ModelState.IsValid)
            {
                if (repo.AdmAuthentication(admin.Login, admin.Password)
                || repo.EdAuthentication(admin.Login, admin.Password))
                {
                    return RedirectToAction("ViewParts", "Part");
                }

                ViewBag.Error = "Логин или пароль неверны!";
                return View(admin);
            }

            return View(admin);
        }
    }
}
