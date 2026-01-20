using Base.Application.Commands;
using Base.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Interfaces
{
    public interface IRoleService
    {
        Task<RoleResponseDto> CreateRoleAsync(CreateRoleCommand command);
        Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync();
    }

}
