using Microsoft.AspNetCore.Mvc;
using ItemManagementAPI.Models;
using ItemManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace ItemManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ItemsController : ControllerBase
    {
        private readonly ItemService _itemService;

        public ItemsController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public ActionResult<List<Item>> Get() => _itemService.Get();

       [Authorize(Roles = "Admin")]
        [HttpGet("{id}", Name = "GetItem")]
        public ActionResult<Item> Get(string id)
        {
            var item = _itemService.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        //[Authorize(Policy = "AdminPolicy")]
         [Authorize(Roles = "User,Admin")] //13.11.24
        [HttpPost]
        public ActionResult<Item> Create(Item item)
        {
            _itemService.Create(item);
            return Ok(new
            {
                Message = "Item Created successfully"
            });
        }
        //[Authorize(Policy = "AdminPolicy")]
        [Authorize(Roles = "Admin")] //13.11.24
        [HttpPut("{id}")]
        public IActionResult Update(string id, Item updatedItem)
        {
            var item = _itemService.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            _itemService.Update(id, updatedItem);
            return NoContent();
        }

        //[Authorize(Policy = "UserPolicy")] //13.11.24
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var item = _itemService.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            _itemService.Remove(id);
            return Ok();
        }
    }
}