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
        public CrewController(UserContext _db, ILogger<CrewService> _logger)
        {
            data = _db;
            logger = _logger;
        }



        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ViewUsers()
        {
            CrewService service = new CrewService(data, logger);
            var users = await service.GetCrewOrUsersAsync("users");
            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "editor")]
        public async Task<IActionResult> ViewCrewOrUsers()
        {
            CrewService service = new CrewService(data, logger);
            var users = await service.GetCrewOrUsersAsync("users");
            return View(users);
        }

        [HttpPost]
        [Authorize(Roles = "editor")]
        public async Task<IActionResult> ViewCrewOrUsers(string role)
        {
            CrewService service = new CrewService(data, logger);
            var crew = await service.GetCrewOrUsersAsync(role);
            return View(crew);
        }



        [HttpGet]
        [Authorize(Roles = "editor")]
        public IActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "editor")]
        public async Task<IActionResult> AddAdmin(string name, string password)
        {
            CrewService service = new CrewService(data, logger);

            if(await service.AddAdminAsync(name, password))
            {
                return RedirectToAction("ViewCrewOrUsers");
            }
            else
            {
                return BadRequest();
            }
        }



        [HttpDelete]
        [Route("api/crew/remove")]
        [Authorize(Roles = "admin, editor")]
        public async Task<IActionResult> DeleteCrewAsync(int id, string role)
        {
            CrewService service = new CrewService(data, logger);

            var crew = await service.RemoveCrewAsync(id, role);

            if (crew != null)
            {
                return Json(crew);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
