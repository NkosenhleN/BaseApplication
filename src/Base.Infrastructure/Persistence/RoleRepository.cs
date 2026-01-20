using Base.Domain.Entities;
using Base.Domain.Interfaces;
using Base.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infrastructure.Persistence
{
    public class RoleRepository : IRoleRepository
    {
        private readonly BaseDbContext _context;

        public RoleRepository(BaseDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetByNameAsync(string name)
            => await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);

        public async Task<IEnumerable<Role>> GetAllAsync()
            => await _context.Roles.ToListAsync();

        public async Task AddAsync(Role role)
            => await _context.Roles.AddAsync(role);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }

}
