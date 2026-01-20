using Base.API.DTOs;
using Base.Application.Commands;

namespace Base.API.Mappers
{
    public static class CreateRoleDtoMapper
    {
        public static CreateRoleCommand ToCommand(this CreateRoleDto dto)
            => new CreateRoleCommand(
                dto.Name
            );
    }
}
