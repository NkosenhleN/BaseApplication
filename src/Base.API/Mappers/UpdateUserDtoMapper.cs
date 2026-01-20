using Base.API.DTOs;
using Base.Application.Commands;

namespace Base.API.Mappers
{
    public static class UpdateUserDtoMapper
    {
        public static UpdateUserCommand ToCommand(this UpdateUserDto dto)
            => new UpdateUserCommand(
                dto.FirstName,
                dto.LastName,
                dto.Email,
                dto.IsActive
            );
    }
}


