
using Base.Application.Commands;
using Base.Domain.Entities;
using Base.Application.Responses;

namespace Base.Application.Mappings
{
    public static class UserMappings
    {
        //Entity -> Response DTO
        public static UserResponse ToResponseDto(this User user)
            => new UserResponse
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                IsLocked = user.IsLocked,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                PasswordChangedAt = user.PasswordChangedAt
            };

        // Command -> Entity (used internally in service)
        public static User ToEntity(this CreateUserCommand command, byte[] hash, byte[] salt)
            => new User(
                command.UserName,
                command.FirstName,
                command.LastName,
                command.Email,
                hash,
                salt
            );

    }
}

