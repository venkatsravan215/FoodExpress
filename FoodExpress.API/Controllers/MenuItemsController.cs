using FoodExpress.Core.DTOs;
using FoodExpress.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FoodExpress.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemsController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemResponseDto>>> GetAll()
        {
            var items = await _menuItemService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MenuItemResponseDto>> GetById(int id)
        {
            var item = await _menuItemService.GetByIdAsync(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<MenuItemResponseDto>> Create(
            MenuItemCreateDto menuItemDto)
        {
            var createdItem =
                await _menuItemService.CreateAsync(menuItemDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdItem.Id },
                createdItem);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            MenuItemCreateDto menuItemDto)
        {
            var existingItem = await _menuItemService.GetByIdAsync(id);

            if (existingItem == null)
                return NotFound();

            await _menuItemService.UpdateAsync(id, menuItemDto);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingItem = await _menuItemService.GetByIdAsync(id);

            if (existingItem == null)
                return NotFound();

            await _menuItemService.DeleteAsync(id);

            return NoContent();
        }
    }
}
