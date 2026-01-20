using Base.Application.Commands;
using Base.Application.Responses;
using Base.Domain.Common.Pagination;
using Base.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> CreateUserAsync(CreateUserCommand command);
        Task<UserResponseDto> UpdateUserAsync(Guid userId, UpdateUserCommand command);
        Task<UserResponseDto?> GetByIdAsync(Guid id);
        Task<PagedResult<UserResponseDto>> GetUsersAsync(GetUsersQuery query);
        Task DeleteUserAsync(Guid userId);
        Task<UserResponseDto> ChangePasswordAsync(ChangePasswordCommand command);
        Task AssignRoleAsync(Guid userId, string roleName);
    }
}
