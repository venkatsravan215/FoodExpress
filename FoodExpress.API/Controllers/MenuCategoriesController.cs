using FoodExpress.Core.DTOs;
using FoodExpress.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FoodExpress.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MenuCategoriesController : ControllerBase
    {
        private readonly IMenuCategoryService _categoryService;

        public MenuCategoriesController(
            IMenuCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            MenuCategoryCreateDto categoryDto)
        {
            var createdCategory =
                await _categoryService.CreateAsync(categoryDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdCategory.Id },
                createdCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            MenuCategoryCreateDto categoryDto)
        {
            var existingCategory =
                await _categoryService.GetByIdAsync(id);

            if (existingCategory == null)
                return NotFound();

            await _categoryService.UpdateAsync(id, categoryDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingCategory =
                await _categoryService.GetByIdAsync(id);

            if (existingCategory == null)
                return NotFound();

            await _categoryService.DeleteAsync(id);

            return NoContent();
        }
    }
}
