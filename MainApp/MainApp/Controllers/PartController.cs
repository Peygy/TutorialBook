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
        private PartsService partService;

        public PartController(TopicsContext _db, ILogger<PartsService> _logger)
        {
            data = _db;
            logger = _logger;  
        }



        [HttpGet]
        public async Task<IActionResult> ViewParts(int parentId, string parentName, string table)
        {
            partService = new PartsService(data, logger);

            var parts = await partService.GetPartsAsync(parentId, table);
            ViewBag.ParentId = parentId;
            ViewBag.Table = table;
            ViewBag.ParentName = parentName;
            return View(parts);
        }

        [HttpGet]
        public async Task<IActionResult> ViewPost(int postId)
        {
            partService = new PartsService(data, logger);

            var post = await partService.GetPartAsync(postId, "subchapter");
            if (post != null)
            {
                return View(post);
            }

            return BadRequest();
        }



        [HttpGet]
        public IActionResult AddPart(int parentId, string parentName, string table)
        {
            ViewBag.ParentId = parentId;
            ViewBag.ParentName = parentName;
            ViewBag.Table = table;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPart(
            int parentId, 
            string parentName, 
            string partName, 
            string content, 
            string table)
        {
            partService = new PartsService(data, logger);

            if (await partService.AddPartAsync(parentId, partName, content, table))
            {
                return RedirectToAction("ViewParts", new { parentId, parentName, table });
            }

            return BadRequest();
        }



        [HttpGet]
        public async Task<IActionResult> UpdatePart(int partId, string table, string parentTable)
        {
            partService = new PartsService(data, logger);

            var part = await partService.GetPartAsync(partId, table);
            if(table != "section")
            {
                ViewBag.Parents = await partService.GetPartsParentsAsync(partId, table);              
            }

            ViewBag.ParentTable = parentTable;

            return View(part);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePart(
            int partId, 
            int parentId, 
            string newName, 
            string content, 
            string parentName, 
            string table, 
            string parentTable)
        {
            partService = new PartsService(data, logger);

            if (await partService.UpdatePartAsync(partId, parentId, newName, content, table))
            {
                return RedirectToAction("ViewParts", new { parentId, parentName, parentTable });
            }

            return BadRequest();
        }



        [HttpDelete]
        [Route("/api/part/remove")]
        public async Task<IActionResult> DeletePartAsync(int partId, string table)
        {
            partService = new PartsService(data, logger);

            var part = await partService.RemovePartAsync(partId, table);
            if (part != null) return Json(part);
            return BadRequest();
        }
    }
}
