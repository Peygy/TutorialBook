using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MainApp.Services;

namespace MainApp.Controllers
{
    // Controller to manage tutorial pages
    public class PageController : Controller
    {
        public IActionResult Welcome()
        {
            return View();
        }

        public IActionResult Study()
        {
            return View();
        }

        [Authorize(Roles = "admin, editor")]
        public IActionResult CrewPanel()
        {
            return PartialView();
        }

        [Authorize(Roles = "user")]
        public IActionResult ViewProfile()
        {
            return View();
        }
    }
}
