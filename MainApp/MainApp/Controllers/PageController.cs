using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainApp.Controllers
{
    public class PageController : Controller
    {
        [Route("/study")]
        [Authorize(Roles = "admin, user, creator")]
        public IActionResult StudyPage()
        {
            return View();
        }
    }
}
