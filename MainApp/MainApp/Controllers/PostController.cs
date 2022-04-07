using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MainApp.Services;

namespace MainApp.Controllers
{
    public class PostController : Controller
    {
        [HttpGet]
        [Route("/api/parts/get/{table:string}")]
        [Authorize(Roles = "admin, creator")]
        public async Task<IActionResult> ViewParts(int id, string name, string table)
        {
            BroadcastService broadcastService = new BroadcastService();
            var part = await broadcastService.GetPartAsync_Db(id, name, table);

            if (part != null)
            {
                return Json(part);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("/api/parts/add/{table:string}")]
        [Authorize(Roles = "admin, creator")]
        public async Task<IActionResult> AddPart(string name, int parentPartId, string parentPartName, string content, string table)
        {
            BroadcastService broadcastService = new BroadcastService();
            var part = await broadcastService.AddPartAsync_Db(name, parentPartId, parentPartName, content, table);

            if (part != null)
            {
                return Json(part);
            }
            else
            {
                return BadRequest(new { message = "Пользователь не найден" });
            }
        }

        [HttpPut]
        [Route("/api/parts/update/{table:string}")]
        [Authorize(Roles = "admin, creator")]
        public async Task<IActionResult> UpdatePart(int id, string name, string newName, string newContent, string table)
        {
            BroadcastService broadcastService = new BroadcastService();
            var part = await broadcastService.UpdatePartAsync_Db(id, name, newName, newContent, table);

            if (part != null)
            {
                return Json(part);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("/api/parts/remove/{table:string}")]
        [Authorize(Roles = "admin, creator")]
        public async Task<IActionResult> DeletePart(int id, string name, string table)
        {
            BroadcastService broadcastService = new BroadcastService();
            var part = await broadcastService.RemovePartAsync_Db(id, name, table);

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
