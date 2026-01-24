using Base.API.DTOs;
using Base.Application.Commands;

namespace Base.API.Mappers
{
    public static class LoginDtoMapper
    {
        public static LoginCommand ToCommand(this LoginDto dto)
            => new LoginCommand(
                dto.UserName,
                dto.Password
            );
    }
}
