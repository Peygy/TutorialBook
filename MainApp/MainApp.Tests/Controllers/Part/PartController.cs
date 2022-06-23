using Microsoft.AspNetCore.Mvc;
using MainApp.Tests.Models.Repositories;

namespace MainApp.Tests.Controllers.Part
{
    // A imitation of the PartController from the MainApp for controller testing
    public class PartController : Controller
    {
        // Interface with method imitations for controller working
        private readonly IPartRepository repo;

        public PartController(IPartRepository _repo)
        {
            repo = _repo;
        }



        public IActionResult ViewParts(int id, string parentName, string table)
        {
            if (table == null)
            {
                var parts = repo.GetParts(0, "onload");
                ViewBag.Name = "section";
                return View(parts);
            }

            if (table == "subchapter")
            {
                var posts = repo.GetPosts(id);
                if (posts != null)
                {
                    ViewBag.Name = parentName;
                    return View(posts);
                }
            }
            else
            {
                var parts = repo.GetParts(id, table);
                if (parts != null)
                {
                    ViewBag.Name = parentName;
                    return View(parts);
                }
            }

            return BadRequest();
        }

        public IActionResult ViewPost(int id)
        {
            var post = repo.GetPart(id, "subchapter");
            if (post != null)
            {
                return View(post);
            }

            return BadRequest();
        }



        [HttpGet]
        public IActionResult AddPart(int parentId, string parentName, string table)
        {
            var part = new List<object[]> { new object[] { parentId, parentName, table } };
            return View(part);
        }

        [HttpPost]
        public IActionResult AddPart(string partName, int parentId, string table)
        {
            if (repo.AddPartRepo(parentId, partName, table))
            {
                return RedirectToAction("ViewParts", new List<object[]> { new object[] { parentId, table } });
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult AddPost(int parentId, string parentName)
        {
            var part = new List<object[]> { new object[] { parentId, parentName } };
            return View(part);
        }

        [HttpPost]
        public IActionResult AddPost(string postName, string content, int parentId)
        {
            if (repo.AddPostRepo(parentId, postName, content))
            {
                return RedirectToAction("ViewParts", new List<object[]> { new object[] { parentId, "subchapter" } });
            }

            return BadRequest();
        }





        [HttpGet]
        public IActionResult UpdatePart(int id, string table)
        {
            var part = repo.GetPart(id, table);
            if (table != "section")
            {
                ViewBag.Parents = repo.GetParts(id, table + "parents");
            }

            return View(part);
        }

        [HttpPost]
        public IActionResult UpdatePart(string newName, int partId, int parentId, string table)
        {
            if (repo.UpdatePartRepo(partId, parentId, newName, String.Empty, table))
            {
                return RedirectToAction("ViewParts", new List<object[]> { new object[] { parentId, table } });
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult UpdatePost(int id)
        {
            var post = repo.GetPart(id, "post");
            ViewBag.Parents = repo.GetParts(id, "postparents");

            return View(post);
        }

        [HttpPost]
        public IActionResult UpdatePost(string newName, string content, int partId, int parentId, string table)
        {
            if (repo.UpdatePartRepo(partId, parentId, newName, content, table))
            {
                return RedirectToAction("ViewParts", new List<object[]> { new object[] { parentId, table } });
            }

            return BadRequest();
        }



        public IActionResult DeletePart(int id, string table)
        {
            var part = repo.RemovePart(id, table);
            if (part != null) return Json(part);
            return BadRequest();
        }

        public IActionResult DeletePost(int id)
        {
            var post = repo.RemovePost(id);
            if (post != null) return Json(post);
            return BadRequest();
        }
    }
}
