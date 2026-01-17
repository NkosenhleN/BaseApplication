using Base.API.DTOs;
using Base.Application.Commands;

namespace Base.API.Mappers
{
    public static class CreateUserDtoMapper
    {
        public static CreateUserCommand ToCommand(this CreateUserDto dto)
            => new CreateUserCommand(
                dto.UserName,
                dto.FirstName,
                dto.LastName,
                dto.Email,
                dto.Password
            );
    }
}


