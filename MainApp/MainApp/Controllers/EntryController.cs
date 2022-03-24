using Microsoft.AspNetCore.Mvc;
using MainApp.Models;

namespace MainApp.Controllers
{
    public class EntryController : Controller
    {  


        public IActionResult CookieCheck(HttpContext httpContext)
        {
            if (!httpContext.Request.Cookies.ContainsKey("login"))
            {
                return Redirect("~/Entry/SignIn");
            }
            else
            {
                return Redirect("~/Home/Welcome");
            }
        }


        public string SignIn()
        {
            HttpContext.Response.Cookies.Append("login", "Tom");
            return "SignIn";
        }
    }
}
