using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MainApp.Services;


namespace MainApp.Controllers
{
    public class CrewController : Controller
    {
        // Methods for managing admins and users for the editor
        // And methods that are included in the list of admin methods

        // Display all users for admin
        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route("adm/control/users")]
        public async Task<IActionResult> ViewUsers()
        {
            CrewService service = new CrewService();
            var users = await service.GetCrewOrUsersAsync("users");
            return View(users);
        }

        // Display all users and admins for editor
        [HttpGet]
        [Authorize(Roles = "creator")]
        [Route("ed/control/crew")]
        public async Task<IActionResult> ViewCrewOrUsers()
        {
            CrewService service = new CrewService();
            var users = await service.GetCrewOrUsersAsync("users");
            return View(users);
        }

        [HttpPost]
        [Authorize(Roles = "creator")]
        [Route("ed/control/crew")]
        public async Task<IActionResult> ViewCrewOrUsers(string role)
        {
            CrewService service = new CrewService();
            var crew = await service.GetCrewOrUsersAsync(role);
            return View(crew);
        }



        // Adding a new admin to control the website
        [HttpGet]
        [Authorize(Roles = "creator")]
        [Route("ed/control/addcrew")]
        public IActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "creator")]
        [Route("ed/control/addcrew")]
        public async Task<IActionResult> AddAdmin(string name, string password)
        {
            CrewService service = new CrewService();

            if(await service.AddAdminAsync(name, password))
            {
                return RedirectToAction("ViewCrewOrUsers");
            }
            else
            {
                return BadRequest();
            }
        }



        // Remove admin and user if needed
        [HttpDelete]
        [Route("api/crew/remove}")]
        [Authorize(Roles = "admin, editor")]
        public async Task<IActionResult> DeleteCrewAsync(int id, string role)
        {
            CrewService service = new CrewService();

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
