using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MainApp.Services;
using MainApp.Models;

namespace MainApp.Controllers
{
    // Controller to manage tutorial parts 
    [Authorize(Roles = "admin, editor")]
    public class PartController : Controller
    {
        // Data context for parts
        private TopicsContext data;
        // Logger for exceptions
        private ILogger<PartsService> logger;
        public PartController(TopicsContext _db, ILogger<PartsService> _logger)
        {
            data = _db;
            logger = _logger;
        }



        [HttpGet]
        public async Task<IActionResult> ViewParts(int id, string parentName, string table)
        {
            PartsService partService = new PartsService(data, logger);

            if (table == "subchapter")
            {
                var parts = await partService.GetPostsAsync(id);
                if (parts != null)
                {
                    ViewBag.Name = parentName;
                    return View(parts);
                }
            }
            else
            {
                var parts = await partService.GetPartsAsync(id, table);
                if (parts != null)
                {
                    ViewBag.Name = parentName;
                    return View(parts);
                }
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> ViewPost(int id)
        {
            PartsService partService = new PartsService(data, logger);

            var post = await partService.GetPartAsync(id, "subchapter");
            if (post != null)
            {
                return View(post);
            }

            return BadRequest();
        }



        [HttpGet]
        public IActionResult AddPart(int parentId, string parentName, string table)
        {
            var part = new { parentId, parentName, table };
            return View(part);
        }

        [HttpPost]
        public async Task<IActionResult> AddPart(string partName)
        {
            PartsService partService = new PartsService(data, logger);

            var form = HttpContext.Request.Form;
            int.TryParse(form["parentId"], out int parentId);
            string table = form["table"];

            if (await partService.AddPartAsync(parentId, partName, table))
            {
                return RedirectToAction("ViewParts", new { id = parentId, table = table });
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult AddPost(int parentId, string parentName)
        {
            var part = new { parentId, parentName };
            return View(part);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(string postName, string content)
        {
            PartsService partService = new PartsService(data, logger);

            var form = HttpContext.Request.Form;
            int.TryParse(form["parentId"], out int parentId);

            if (await partService.AddPostAsync(parentId, postName, content))
            {
                return RedirectToAction("ViewParts", new { id = parentId, table = "subchapter" });
            }

            return BadRequest();
        }





        [HttpGet]
        public async Task<IActionResult> UpdatePart(int id, string table)
        {
            PartsService partService = new PartsService(data, logger);

            var part = await partService.GetPartAsync(id, table);
            if(table != "section")
            {
                ViewBag.Parents = await partService.GetPartsAsync(id, table + "parents");
            }

            return View(part);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePart(string newName)
        {
            PartsService partService = new PartsService(data, logger);

            var form = HttpContext.Request.Form;
            int.TryParse(form["partId"], out int partId);
            int.TryParse(form["parentId"], out int parentId);
            string table = form["table"];

            if (await partService.UpdatePartAsync(partId, parentId, newName, String.Empty, table))
            {
                return RedirectToAction("ViewParts", new { id = parentId, table = table });
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost(string newName, string content)
        {
            PartsService partService = new PartsService(data, logger);

            var form = HttpContext.Request.Form;
            int.TryParse(form["partId"], out int partId);
            int.TryParse(form["parentId"], out int parentId);
            string table = form["table"];

            if (await partService.UpdatePartAsync(partId, parentId, newName, content, table))
            {
                return RedirectToAction("ViewParts", new { id = parentId, table = table });
            }

            return BadRequest();
        }



        [HttpDelete]
        [Route("/api/parts/remove")]
        public async Task<IActionResult> DeletePartAsync(int id, string table)
        {
            PartsService partService = new PartsService(data, logger);

            var part = await partService.RemovePartAsync(id, table);
            if (part != null)
            {
                return Json(part);
            }

            return BadRequest();
        }
    }
}
