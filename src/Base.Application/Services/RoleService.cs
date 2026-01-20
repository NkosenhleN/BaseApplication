using Base.Application.Commands;
using Base.Application.Interfaces;
using Base.Application.Responses;
using Base.Domain.Entities;
using Base.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleResponseDto> CreateRoleAsync(CreateRoleCommand command)
        {
            var existing = await _roleRepository.GetByNameAsync(command.Name);
            if (existing != null)
                throw new InvalidOperationException("Role already exists");

            var role = new Role(command.Name);
            await _roleRepository.AddAsync(role);
            await _roleRepository.SaveChangesAsync();

            return new RoleResponseDto { Id = role.Id, Name = role.Name };
        }


        public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return roles.Select(r => new RoleResponseDto
            {
                Id = r.Id,
                Name = r.Name
            });
        }
    }
}
