using Microsoft.AspNetCore.Mvc;
using MainApp.Models;

namespace MainApp.Controllers
{
    public class EntryController : Controller
    {        
        public string SignIn()
        {
            //HttpContext.Response.Cookies.Append("login", "Tom");
            return "SignIn";
        }
    }
}
