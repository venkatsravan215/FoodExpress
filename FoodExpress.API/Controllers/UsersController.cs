
using FoodExpress.Core.DTOs;
using FoodExpress.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodExpress.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
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

        // GET: api/Users
        // Temporarily restrict this endpoint to administrators.
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var loggedInUserId = GetLoggedInUserId();

            if (id != loggedInUserId)
                return NotFound();

            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // GET: api/Users/email/example@email.com
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var loggedInUserId = GetLoggedInUserId();

            var loggedInUser =
                await _userService.GetByIdAsync(loggedInUserId);

            if (loggedInUser == null ||
                !string.Equals(
                    loggedInUser.Email,
                    email,
                    StringComparison.OrdinalIgnoreCase))
            {
                return NotFound();
            }

            var user = await _userService.GetByEmailAsync(email);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/Users
        // Registration remains public.
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] UserCreateDto userDto)
        {
            try
            {
                var createdUser =
                    await _userService.CreateAsync(userDto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdUser.Id },
                    createdUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UserUpdateDto userDto)
        {
            var loggedInUserId = GetLoggedInUserId();

            if (id != loggedInUserId)
                return NotFound();

            var existingUser =
                await _userService.GetByIdAsync(id);

            if (existingUser == null)
                return NotFound();

            try
            {
                await _userService.UpdateAsync(id, userDto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Users/5
        // Disabled for ordinary users until an admin-only deletion flow exists.
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                new { message = "Account deletion is not available through this endpoint." });
        }
    }
}
