using Base.API.DTOs;
using Base.API.Extensions;
using Base.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Base.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Map DTO → Domain
            var userEntity = dto.ToDomain();  
            
            var createdUser = await _userService.CreateUserAsync(userEntity);

            // Map Domain → DTO
            var userReadDto = createdUser.ToReadDto();    

            return Ok(userReadDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            // Map Domain → DTO
            var usersDto = users.ToReadDtoList();    
            
            return Ok(usersDto);
        }
    }
}
