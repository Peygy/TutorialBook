using Microsoft.AspNetCore.Mvc;

namespace MainApp.Controllers
{
    public class CrewController : Controller
    {
        public IActionResult ViewUsers()
        {
            return View();
        }
    }
}
