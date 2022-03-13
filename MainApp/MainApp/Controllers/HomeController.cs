using Microsoft.AspNetCore.Mvc;
using MainApp.Models;

namespace MainApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
