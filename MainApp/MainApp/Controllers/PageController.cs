using Microsoft.AspNetCore.Mvc;

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

        public IActionResult ViewProfile()
        {
            return View();
        }

        [Route("/error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            return View(statusCode);
        }
    }
}
