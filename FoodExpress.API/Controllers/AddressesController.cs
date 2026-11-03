
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
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
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

        // GET: api/Addresses/user/5
        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<AddressResponseDto>>>
            GetByUserId(int userId)
        {
            var loggedInUserId = GetLoggedInUserId();

            if (userId != loggedInUserId)
                return NotFound();

            var addresses =
                await _addressService.GetByUserIdAsync(loggedInUserId);

            return Ok(addresses);
        }

        // GET: api/Addresses/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AddressResponseDto>> GetById(int id)
        {
            var userId = GetLoggedInUserId();

            var address = await _addressService.GetByIdAsync(id);

            if (address == null || address.UserId != userId)
                return NotFound();

            return Ok(address);
        }

        // POST: api/Addresses
        [HttpPost]
        public async Task<ActionResult<AddressResponseDto>> Create(
            [FromBody] AddressCreateDto addressDto)
        {
            var userId = GetLoggedInUserId();

            addressDto.UserId = userId;

            var createdAddress =
                await _addressService.CreateAsync(addressDto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdAddress.Id },
                createdAddress);
        }

        // PUT: api/Addresses/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] AddressCreateDto addressDto)
        {
            var userId = GetLoggedInUserId();

            var address = await _addressService.GetByIdAsync(id);

            if (address == null || address.UserId != userId)
                return NotFound();

            addressDto.UserId = userId;

            try
            {
                await _addressService.UpdateAsync(id, addressDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE: api/Addresses/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetLoggedInUserId();

            var address = await _addressService.GetByIdAsync(id);

            if (address == null || address.UserId != userId)
                return NotFound();

            await _addressService.DeleteAsync(id);

            return NoContent();
        }
    }
}
