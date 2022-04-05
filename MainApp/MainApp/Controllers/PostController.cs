using Microsoft.AspNetCore.Mvc;
using MainApp.Models;
using MainApp.Services;

namespace MainApp.Controllers
{
    public class PostController : Controller
    {
        [HttpGet]
        [Route("api/parts/get/{table:string}")]
        public async Task<IActionResult> ViewParts(int id, string name, string table)
        {
            BroadcastService broadcastService = new BroadcastService();
            var part = await broadcastService.GetPart_Db(id, name, table);

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
        [Route("api/parts/add/{table:string}")]
        public async Task<IActionResult> AddPart(string name, int parentPartId, string parentPartName, string table)
        {
            BroadcastService broadcastService = new BroadcastService();
            var part = await broadcastService.AddPart_Db(name, parentPartId, parentPartName, table);

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
        [Route("api/parts/update/{table:string}")]
        public async Task<IActionResult> UpdatePart(int id, string name, string newName, string table)
        {
            BroadcastService broadcastService = new BroadcastService();
            var part = await broadcastService.UpdatePart_Db(id, name, newName, table);

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
        [Route("api/parts/remove/{table:string}")]
        public async Task<IActionResult> DeletePart(int id, string name, string table)
        {
            BroadcastService broadcastService = new BroadcastService();
            var part = await broadcastService.RemovePart_Db(id, name, table);

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
