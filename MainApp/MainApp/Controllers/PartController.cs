using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MainApp.Services;

namespace MainApp.Controllers
{
    public class PartController : Controller
    {
        // При помощи AnchorTagHelper передачу параметров

        [HttpGet]
        [Route("adm/control/partview")]
        [Route("ed/control/partview")]
        [Authorize(Roles = "admin, editor")]
        public async Task<IActionResult> ViewPartsAsync(int id, string table)
        {
            PartsService broadcastService = new PartsService();
            var part = await broadcastService.GetPartsAsync_Db(id, table);

            if (part != null)
            {
                return View(part);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("adm/control/postview")]
        [Route("ed/control/postview")]
        [Authorize(Roles = "admin, editor")]
        public async Task<IActionResult> ViewPostAsync(int id)
        {
            PartsService broadcastService = new PartsService();
            var part = await broadcastService.GetPartAsync_Db(id, "subchapter");

            if (part != null)
            {
                return View(part);
            }
            else
            {
                return BadRequest();
            }
        }





        [HttpGet]
        [Route("adm/control/parts/addpart")]
        [Route("ed/control/parts/addpart")]
        [Authorize(Roles = "admin, editor")]
        public IActionResult AddPart(int parentId, string parentName, string table)
        {
            var part = new { parentId, parentName, table };
            return View(part);
        }

        [HttpPost]
        [Route("adm/control/parts/addpart")]
        [Route("ed/control/parts/addpart")]
        [Authorize(Roles = "admin, editor")]
        public async Task<IActionResult> AddPartAsync(string partName)
        {
            PartsService broadcastService = new PartsService();

            var form = HttpContext.Request.Form;
            int.TryParse(form["parentId"], out int parentId);
            string table = form["table"];

            if (await broadcastService.AddPartAsync_Db(parentId, partName, table))
            {
                return RedirectToAction("ViewPartsAsync", new { id = parentId, table = table });
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("adm/control/parts/addpost")]
        [Route("ed/control/parts/addpost")]
        [Authorize(Roles = "admin, editor")]
        public IActionResult AddPost(int parentId, string parentName)
        {
            var part = new { parentId, parentName };
            return View(part);
        }

        [HttpPost]
        [Route("adm/control/parts/addpost")]
        [Route("ed/control/parts/addpost")]
        [Authorize(Roles = "admin, editor")]
        public async Task<IActionResult> AddPostAsync(string postName, string content)
        {
            PartsService broadcastService = new PartsService();

            var form = HttpContext.Request.Form;
            int.TryParse(form["parentId"], out int parentId);

            if (await broadcastService.AddPostAsync_Db(parentId, postName, content))
            {
                return RedirectToAction("ViewPartsAsync", new { id = parentId, table = "subchapter" });
            }
            else
            {
                return BadRequest();
            }
        }





        [HttpGet]
        [Route("adm/control/parts/updatepart")]
        [Route("ed/control/parts/updatepart")]
        [Authorize(Roles = "admin, editor")]
        public async Task<IActionResult> UpdatePartAsync(int id, string table)
        {
            PartsService broadcastService = new PartsService();

            var part = await broadcastService.GetPartAsync_Db(id, table);

            if(table != "section")
            {
                ViewBag.Parents = await broadcastService.GetPartsAsync_Db(id, table + "s");
            }

            return View(part);
        }

        [HttpPut]
        [Route("adm/control/parts/updatepart")]
        [Route("ed/control/parts/updatepart")]
        [Authorize(Roles = "admin, editor")]
        public async Task<IActionResult> UpdatePartAsync(string newName)
        {
            PartsService broadcastService = new PartsService();

            var form = HttpContext.Request.Form;
            int.TryParse(form["partId"], out int partId);
            int.TryParse(form["parentId"], out int parentId);
            string table = form["table"];

            if (await broadcastService.UpdatePartAsync_Db(partId, parentId, newName, String.Empty, table))
            {
                return RedirectToAction("ViewPartsAsync", new { id = parentId, table = table });
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("adm/control/parts/updatepost")]
        [Route("ed/control/parts/updatepost")]
        [Authorize(Roles = "admin, editor")]
        public async Task<IActionResult> UpdatePostAsync(string newName, string content)
        {
            PartsService broadcastService = new PartsService();

            var form = HttpContext.Request.Form;
            int.TryParse(form["partId"], out int partId);
            int.TryParse(form["parentId"], out int parentId);
            string table = form["table"];

            if (await broadcastService.UpdatePartAsync_Db(partId, parentId, newName, content, table))
            {
                return RedirectToAction("ViewPartsAsync", new { id = parentId, table = table });
            }
            else
            {
                return BadRequest();
            }
        }





        [HttpDelete]
        [Route("api/parts/remove}")]
        [Authorize(Roles = "admin, editor")]
        public async Task<IActionResult> DeletePartAsync(int id, string table)
        {
            PartsService broadcastService = new PartsService();

            var part = await broadcastService.RemovePartAsync_Db(id, table);

            if (part != null)
            {
                return Json(part);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
