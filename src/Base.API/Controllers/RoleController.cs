using Base.API.DTOs;
using Base.API.Mappers;
using Base.Application.Interfaces;
using Base.Application.Responses;
using Base.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Base.API.Controllers
{
    [ApiController]
    [Route("api/roles")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public RoleController(IRoleService roleService, IUserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RoleResponseDto>> CreateRole(CreateRoleDto dto)
        {
            try
            {
                var command = dto.ToCommand();

                var role = await _roleService.CreateRoleAsync(command);
                return CreatedAtAction(nameof(GetAllRoles), new { id = role.Id }, role);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<RoleResponseDto>>> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpPut("{id:guid}/assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(Guid id, AssignRoleDto dto)
        {
            try
            {
                await _userService.AssignRoleAsync(id, dto.RoleName);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{userId}/roles/{roleName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveRole(Guid userId, string roleName)
        {
            await _userService.RemoveRoleAsync(userId, roleName);
            return NoContent();
        }

    }
}
