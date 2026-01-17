using Base.API.DTOs;
using Base.Application.Commands;

namespace Base.API.Mappers
{
    public static class ChangePasswordMapper
    {
        public static ChangePasswordCommand ToCommand(this ChangePasswordDto dto)
            => new ChangePasswordCommand(
                dto.UserId,
                dto.CurrentPassword, 
                dto.NewPassword
            );
    }
}
