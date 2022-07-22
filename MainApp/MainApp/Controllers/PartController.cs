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
        public async Task<IActionResult> ViewParts(int parentId, string table)
        {
            partService = new PartsService(data, logger);

            if(parentId==0 && table==null)
            {
                table = "onload";
            }

            var parts = await partService.GetPartsAsync(parentId, table);
            var parent = await partService.GetPartAsync(parentId, table);
            if(parent == null)
            {
                return RedirectToAction("Error", "Page");
            }

            ViewBag.ParentId = parentId;
            ViewBag.ParentName = parent.Title;
            ViewBag.Table = table;
            if(table != "onload")
            {
                ViewBag.Parent = await partService.GetPartAsync(parent.ParentId, parent.ParentTable); 
            }

            return View(parts);
        }

        [HttpGet]
        public async Task<IActionResult> ViewPost(int postId)
        {
            partService = new PartsService(data, logger);

            var post = (Post)await partService.GetPartAsync(postId, "post");
            if (post == null)
            {
                return RedirectToAction("Error", "Page");
            }

            ViewBag.Content = await partService.GetContentAsync(post.ContentId);
            return View(post);          
        }



        [HttpGet]
        public async Task<IActionResult> AddPart(int parentId, string table)
        {
            partService = new PartsService(data, logger);

            var parentName = (await partService.GetPartAsync(parentId, table)).Title;    
            return View(new GeneralPart { Id = parentId, Title = parentName, Table = table });
        }

        [HttpPost]
        public async Task<IActionResult> AddPart(
            int parentId, 
            string parentName,
            string table, 
            string partName, 
            string content)
        {
            partService = new PartsService(data, logger);

            if(partName == null)
            {
                return View(new GeneralPart { Id = parentId, Title = parentName, Table = table });
            }
            if (await partService.AddPartAsync(parentId, partName, content, table))
            {
                return RedirectToAction("ViewParts", new { parentId, table });
            }

             return RedirectToAction("AddPart", new { parentId, table });
        }



        [HttpGet]
        public async Task<IActionResult> UpdatePart(int partId, string table)
        {
            partService = new PartsService(data, logger);

            var part = await partService.GetPartAsync(partId, table);
            if (part == null)
            {
                return RedirectToAction("Error", "Page");
            }

            if (table != "section")
            {
                ViewBag.Parents = await partService.GetPartsParentsAsync(partId, table);  
                if(table == "post")
                {
                    ViewBag.Content = await partService.GetContentAsync(((Post)part).ContentId);
                }            
            }

            return View(part);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePart(
            int partId, 
            string newName, 
            string newContent, 
            string table, 
            int newParentId,

            int parentId,  
            string parentTable)
        {
            partService = new PartsService(data, logger);

            if (await partService.UpdatePartAsync(partId, newParentId, newName, newContent, table))
            {           
                return RedirectToAction("ViewParts", new { parentId, table = parentTable });
            }

            return RedirectToAction("UpdatePart", new { partId, table });
        }



        [HttpDelete]
        [Route("/api/part/remove/{table}&{partId:int}")]
        public async Task<IActionResult> DeletePart(int partId, string table)
        {
            partService = new PartsService(data, logger);

            var id = await partService.RemovePartAsync(partId, table);
            return Json(id);
        }
    }
}
