
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
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public PaymentsController(
            IPaymentService paymentService,
            IOrderService orderService)
        {
            _paymentService = paymentService;
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

        private async Task<bool> OwnsOrderAsync(int orderId)
        {
            var userId = GetLoggedInUserId();
            var order = await _orderService.GetByIdAsync(orderId);

            return order != null && order.UserId == userId;
        }

        private async Task<bool> OwnsPaymentAsync(
            PaymentResponseDto payment)
        {
            return await OwnsOrderAsync(payment.OrderId);
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentResponseDto>>> GetAll()
        {
            var userId = GetLoggedInUserId();
            var orders = await _orderService.GetByUserIdAsync(userId);
            var payments = new List<PaymentResponseDto>();

            foreach (var order in orders)
            {
                var orderPayments =
                    await _paymentService.GetByOrderIdAsync(order.Id);

                payments.AddRange(orderPayments);
            }

            return Ok(payments);
        }

        // GET: api/Payments/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PaymentResponseDto>> GetById(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);

            if (payment == null || !await OwnsPaymentAsync(payment))
                return NotFound(new { message = "Payment not found." });

            return Ok(payment);
        }

        // GET: api/Payments/order/5
        [HttpGet("order/{orderId:int}")]
        public async Task<ActionResult<IEnumerable<PaymentResponseDto>>> GetByOrderId(
            int orderId)
        {
            if (!await OwnsOrderAsync(orderId))
                return NotFound();

            var payments =
                await _paymentService.GetByOrderIdAsync(orderId);

            return Ok(payments);
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<ActionResult<PaymentResponseDto>> Create(
            [FromBody] PaymentCreateDto dto)
        {
            if (!await OwnsOrderAsync(dto.OrderId))
                return NotFound(new { message = "Order not found." });

            try
            {
                var payment = await _paymentService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = payment.Id },
                    payment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PATCH: api/Payments/5/status
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}/status")]
        public async Task<ActionResult<PaymentResponseDto>> UpdateStatus(
            int id,
            [FromBody] PaymentStatus status)
        {
            if (!Enum.IsDefined(typeof(PaymentStatus), status))
            {
                return BadRequest(
                    new { message = "Invalid payment status." });
            }

            var existingPayment =
                await _paymentService.GetByIdAsync(id);

            if (existingPayment == null)
            {
                return NotFound(
                    new { message = "Payment not found." });
            }

            try
            {
                var payment =
                    await _paymentService.UpdateStatusAsync(id, status);

                if (payment == null)
                {
                    return NotFound(
                        new { message = "Payment not found." });
                }

                return Ok(payment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        // DELETE: api/Payments/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);

            if (payment == null || !await OwnsPaymentAsync(payment))
                return NotFound(
                    new { message = "Payment not found." });

            var deleted = await _paymentService.DeleteAsync(id);

            if (!deleted)
                return NotFound(
                    new { message = "Payment not found." });

            return NoContent();
        }
    }
}
