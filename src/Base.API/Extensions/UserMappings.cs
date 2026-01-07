using Base.API.DTOs;
using Base.Domain.Entities;

namespace Base.API.Extensions
{
    public static class UserMappings
    {
        // Domain → DTO
        public static UserReadDto ToReadDto(this User user)
        {
            return new UserReadDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Department = user.Department,
                CreatedAt = user.CreatedAt
            };
        }

        // DTO → Domain
        public static User ToDomain(this UserCreateDto dto)
        {
            return new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Department = dto.Department!,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        // Collection mapping
        public static IEnumerable<UserReadDto> ToReadDtoList(this IEnumerable<User> users)
        {
            return users.Select(u => u.ToReadDto());
        }
    }
}

