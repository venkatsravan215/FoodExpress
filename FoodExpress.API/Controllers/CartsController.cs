using FoodExpress.Core.DTOs;
using FoodExpress.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodExpress.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<CartResponseDto>> GetAll()
        {
            var userId = GetLoggedInUserId();
            var cart = await _cartService.GetByUserIdAsync(userId);

            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CartResponseDto>> GetById(int id)
        {
            var userId = GetLoggedInUserId();

            var cart = await _cartService.GetByIdAsync(id);

            if (cart == null || cart.UserId != userId)
                return NotFound();

            return Ok(cart);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<CartResponseDto>> GetByUserId(int userId)
        {
            var loggedInUserId = GetLoggedInUserId();

            if (userId != loggedInUserId)
                return NotFound();

            var cart = await _cartService.GetByUserIdAsync(loggedInUserId);

            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult<CartResponseDto>> Create(
    CartCreateDto cartDto)
        {
            var userId = GetLoggedInUserId();

            cartDto.UserId = userId;

            try
            {
                var createdCart = await _cartService.CreateAsync(cartDto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdCart.Id },
                    createdCart);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetLoggedInUserId();

            var cart = await _cartService.GetByIdAsync(id);

            if (cart == null || cart.UserId != userId)
                return NotFound();

            await _cartService.DeleteAsync(id);

            return NoContent();
        }
        private int GetLoggedInUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out var id))
            {
                throw new UnauthorizedAccessException(
                    "Unable to identify the logged-in user.");
            }

            return id;
        }
    }
}
