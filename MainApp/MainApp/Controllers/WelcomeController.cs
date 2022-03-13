using Microsoft.AspNetCore.Mvc;
using MainApp.Models;

namespace MainApp.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
