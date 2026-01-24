using Base.Domain.Common.Pagination;
using Base.Domain.Entities;
using Base.Domain.Queries;


namespace Base.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUserNameAsync(string userName);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task SaveChangesAsync();
        Task<bool> UsernameExistsAsync(string username);
        Task<PagedResult<User>> GetPagedAsync(GetUsersQuery query);

    }
}
