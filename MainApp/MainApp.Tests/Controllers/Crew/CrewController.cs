using Microsoft.AspNetCore.Mvc;
using MainApp.Tests.Models.Repositories;
using MainApp.Tests.Models.Crew;

namespace MainApp.Tests.Controllers.Crew
{
    // A imitation of the CrewController from the MainApp for controller testing
    public class CrewController : Controller
    {
        // Interface with method imitations for controller working
        private readonly ICrewRepository repo;

        public CrewController(ICrewRepository _repository)
        {
            repo = _repository;
        }



        public IActionResult ViewUsers()
        {
            return View(repo.GetUsers());
        }

        public IActionResult ViewAdmins()
        {
            return View(repo.GetAdmins());
        }



        [HttpGet]
        public IActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAdmin(Admin admin)
        {
            if (ModelState.IsValid)
            {
                if (repo.AddNewAdmin(admin))
                    return RedirectToAction("ViewAdmins");
                else return BadRequest();
            }

            return View(admin);
        }


        public IActionResult DeleteUsers(int id)
        {
            var user = repo.RemoveUser(id);

            if (user != null) return Json(user);
            else return BadRequest();
        }

        public IActionResult DeleteAdmins(int id)
        {
            var admin = repo.RemoveAdmin(id);

            if (admin != null) return Json(admin);
            else return BadRequest();
        }
    }
}
