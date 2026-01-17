using Base.API.DTOs;
using Base.API.Mappers;
using Base.Application.Responses;
using Base.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Base.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(CreateUserDto dto)
        {
            try
            {
                var command = dto.ToCommand();
                var user = await _userService.CreateUserAsync(command);

                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }
        }

        [HttpPut("update-password")]
        public async Task<ActionResult> UpdatePassword(ChangePasswordDto dto)
        {
            try
            {
                var command = dto.ToCommand();
                await _userService.ChangePasswordAsync(command);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();

            if (users == null || !users.Any())
                return NoContent();

            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponse>> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user); 
        }
    }
}
