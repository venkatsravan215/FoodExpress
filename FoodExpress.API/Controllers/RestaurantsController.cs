using FoodExpress.Core.DTOs;
using FoodExpress.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodExpress.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var restaurants = await _restaurantService.GetAllAsync();
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var restaurant = await _restaurantService.GetByIdAsync(id);

            if (restaurant == null)
                return NotFound();

            return Ok(restaurant);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            RestaurantCreateDto restaurantDto)
        {
            var createdRestaurant =
                await _restaurantService.CreateAsync(restaurantDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdRestaurant.Id },
                createdRestaurant);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            RestaurantUpdateDto restaurantDto)
        {
            var existingRestaurant =
                await _restaurantService.GetByIdAsync(id);

            if (existingRestaurant == null)
                return NotFound();

            await _restaurantService.UpdateAsync(id, restaurantDto);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingRestaurant =
                await _restaurantService.GetByIdAsync(id);

            if (existingRestaurant == null)
                return NotFound();

            await _restaurantService.DeleteAsync(id);

            return NoContent();
        }
    }
}
