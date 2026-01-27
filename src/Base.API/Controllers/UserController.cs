using Base.API.DTOs;
using Base.API.Mappers;
using Base.Application.Commands;
using Base.Application.Interfaces;
using Base.Application.Responses;
using Base.Application.Services;
using Base.Domain.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Base.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("user")]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
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
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto dto)
        {
            var command = dto.ToCommand();

            try
            {
                var updatedUser = await _userService.UpdateUserAsync(id, command);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
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
        public async Task<ActionResult<List<UserResponseDto>>> GetUsers([FromQuery] GetUsersQuery query)
        {
            var users = await _userService.GetUsersAsync(query);

            if (users == null || users.TotalCount <=0)
                return NoContent();

            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponseDto>> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user); 
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var command = dto.ToCommand();

            try
            {
                var token = await _userService.LoginAsync(command);
                return Ok(new {Token = token});
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/unlock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnlockUser(Guid id)
        {
            await _userService.UnlockUserAsync(id);
            return NoContent();
        }

    }
}
