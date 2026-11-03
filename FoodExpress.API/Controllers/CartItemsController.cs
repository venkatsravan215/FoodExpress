using Microsoft.AspNetCore.Authorization;
using FoodExpress.Core.DTOs;
using FoodExpress.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodExpress.API.Controllers
{
    [Authorize]
    [Route("api/carts/{cartId:int}/items")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;
        private readonly ICartService _cartService;

        public CartItemsController(
            ICartItemService cartItemService,
            ICartService cartService)
        {
            _cartItemService = cartItemService;
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<ActionResult<CartResponseDto>> AddItem(
            int cartId,
            CartItemCreateDto itemDto)
        {
            if (!await OwnsCartAsync(cartId))
                return NotFound();

            try
            {
                var cart = await _cartItemService.AddItemAsync(
                    cartId, itemDto);

                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{cartItemId:int}")]
        public async Task<IActionResult> UpdateQuantity(
            int cartId,
            int cartItemId,
            [FromBody] int quantity)
        {
            if (!await OwnsCartAsync(cartId))
                return NotFound();
            try
            {
                await _cartItemService.UpdateQuantityAsync(
                    cartId, cartItemId, quantity);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{cartItemId:int}")]
        public async Task<IActionResult> RemoveItem(
            int cartId,
            int cartItemId)
        {
            if (!await OwnsCartAsync(cartId))
                return NotFound();
            try
            {
                await _cartItemService.RemoveItemAsync(
                    cartId, cartItemId);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        private int GetLoggedInUserId()
        {
            var userId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userId, out var id))
                throw new UnauthorizedAccessException(
                    "Unable to identify the logged-in user.");

            return id;
        }

        private async Task<bool> OwnsCartAsync(int cartId)
        {
            var cart = await _cartService.GetByIdAsync(cartId);

            return cart != null &&
                   cart.UserId == GetLoggedInUserId();
        }
    }

}

