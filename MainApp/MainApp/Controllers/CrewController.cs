using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MainApp.Services;
using MainApp.Models;

namespace MainApp.Controllers
{
    // Controller to manage users and crew
    public class CrewController : Controller
    {
        // Data context for users and crew
        private UserContext data;
        // Logger for exceptions
        private ILogger<CrewService> logger;
        private CrewService crewService;

        public CrewController(UserContext _db, ILogger<CrewService> _logger)
        {
            data = _db;
            logger = _logger;
        }



        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ViewUsers()
        {
            crewService = new CrewService(data, logger);
            return View(await crewService.GetUsersAsync());
        }

        [HttpGet]
        [Authorize(Roles = "editor")]
        public async Task<IActionResult> ViewAdmins()
        {
            crewService = new CrewService(data, logger);
            return View(await crewService.GetAdminsAsync());
        }



        [HttpGet]
        [Authorize(Roles = "editor")]
        public IActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "editor")]
        public async Task<IActionResult> AddAdmin(Admin admin)
        {
            crewService = new CrewService(data, logger);

            if (ModelState.IsValid)
            {
                if (await crewService.AddAdminAsync(admin))
                    return RedirectToAction("ViewAdmins");
                else return BadRequest();
            }

            return View(admin);
        }



        [HttpDelete]
        [Route("api/users/remove/{id:int}")]
        [Authorize(Roles = "admin, editor")]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            crewService = new CrewService(data, logger);

            var user = await crewService.RemoveUserAsync(id);
            if (user != null) return Json(user);
            else return View();
        }

        [HttpDelete]
        [Route("api/admins/remove/{id:int}")]
        [Authorize(Roles = "editor")]
        public async Task<IActionResult> DeleteAdmins(int id)
        {
            crewService = new CrewService(data, logger);

            var admin = await crewService.RemoveAdminAsync(id);
            if (admin != null) return Json(admin);
            else return View();
        }
    }
}
