
using FoodExpress.Core.DTOs;
using FoodExpress.Core.Enums;
using FoodExpress.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodExpress.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private int GetLoggedInUserId()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userId, out var id))
            {
                throw new UnauthorizedAccessException(
                    "Unable to identify the logged-in user.");
            }

            return id;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAll()
        {
            var userId = GetLoggedInUserId();

            var orders = await _orderService.GetByUserIdAsync(userId);

            return Ok(orders);
        }

        // GET: api/Orders/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderResponseDto>> GetById(int id)
        {
            var userId = GetLoggedInUserId();

            var order = await _orderService.GetByIdAsync(id);

            if (order == null || order.UserId != userId)
                return NotFound();

            return Ok(order);
        }

        // GET: api/Orders/user/5
        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetByUserId(
            int userId)
        {
            var loggedInUserId = GetLoggedInUserId();

            if (userId != loggedInUserId)
                return NotFound();

            var orders = await _orderService.GetByUserIdAsync(
                loggedInUserId);

            return Ok(orders);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> Create(
            [FromBody] OrderCreateDto orderDto)
        {
            try
            {
                var userId = GetLoggedInUserId();

                // Always use the authenticated user's ID.
                orderDto.UserId = userId;

                var createdOrder = await _orderService.CreateAsync(orderDto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdOrder.Id },
                    createdOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PATCH: api/Orders/5/status
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] OrderStatus status)
        {
            // Only the owner can update their order through this endpoint.
            var userId = GetLoggedInUserId();

            var order = await _orderService.GetByIdAsync(id);

            if (order == null || order.UserId != userId)
                return NotFound();

            try
            {
                await _orderService.UpdateStatusAsync(id, status);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetLoggedInUserId();

            var order = await _orderService.GetByIdAsync(id);

            if (order == null || order.UserId != userId)
                return NotFound();

            await _orderService.DeleteAsync(id);

            return NoContent();
        }
    }
}
