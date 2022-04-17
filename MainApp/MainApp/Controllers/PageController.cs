using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainApp.Controllers
{
    // Controller to manage tutorial pages
    public class PageController : Controller
    {
        public IActionResult WelcomePage()
        {
            return View();
        }

        public IActionResult StudyPage()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult AdmControl()
        {
            return View();
        }

        [Authorize(Roles = "editor")]
        public IActionResult EdControl()
        {
            return View();
        }

        [Authorize(Roles = "user")]
        public IActionResult ViewProfile()
        {
            return View();
        }
    }
}
