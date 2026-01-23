
using Base.Domain.Queries;
using Base.Domain.Common.Pagination;
using Base.Domain.Entities;
using Base.Domain.Interfaces;
using Base.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Base.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BaseDbContext _context;

        public UserRepository(BaseDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users
                .AnyAsync(u => u.UserName == username);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await SaveChangesAsync();
        }

        public async Task<PagedResult<User>> GetPagedAsync(GetUsersQuery query)
        {
            IQueryable<User> users = _context.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                users = users.Where(u =>
                    u.UserName.Contains(query.Search) ||
                    u.Email.Contains(query.Search));
            }

            if (query.IsActive.HasValue)
            {
                users = users.Where(u => u.IsActive == query.IsActive.Value);
            }

            var totalCount = await users.CountAsync();

            var items = await users
                .Skip(query.Skip)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Items = items,
                TotalCount = totalCount,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

    }
}
