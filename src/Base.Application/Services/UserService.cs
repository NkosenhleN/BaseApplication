using Base.Application.Commands;
using Base.Application.Interfaces;
using Base.Application.Mappings;
using Base.Application.Responses;
using Base.Domain.Common.Pagination;
using Base.Domain.Entities;
using Base.Domain.Interfaces;
using Base.Domain.Queries;

namespace Base.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRoleRepository _roleRepository;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher,
            IRoleRepository roleRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _jwtService = jwtService;
        }

        public async Task<UserResponseDto> CreateUserAsync(CreateUserCommand command)
        {
            if (await _userRepository.UsernameExistsAsync(command.UserName))
            {
                throw new InvalidOperationException("Username already exists");
            }

            var (hash, salt) = _passwordHasher.HashPassword(command.Password);

            var user = command.ToEntity(hash, salt);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return user.ToResponseDto();
        }

        public async Task<UserResponseDto> UpdateUserAsync(Guid userId, UpdateUserCommand command)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            if (!string.IsNullOrWhiteSpace(command.FirstName))
                user.FirstName = command.FirstName;

            if (!string.IsNullOrWhiteSpace(command.LastName))
                user.LastName = command.LastName;

            if (!string.IsNullOrWhiteSpace(command.Email))
                user.Email = command.Email;

            if (command.IsActive.HasValue)
                user.Activate();
            else
                user.Deactivate();

                await _userRepository.SaveChangesAsync();

            return user.ToResponseDto();
        }


        public async Task<PagedResult<UserResponseDto>> GetUsersAsync(GetUsersQuery query)
        {
            var result = await _userRepository.GetPagedAsync(query);

            return new PagedResult<UserResponseDto>
            {
                Items = result.Items.Select(u => u.ToResponseDto()).ToList(),
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }


        public async Task<UserResponseDto?> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return null; 

            return user.ToResponseDto();
        }


        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _userRepository.UsernameExistsAsync(username);
        }


        public async Task<UserResponseDto> ChangePasswordAsync(ChangePasswordCommand command)
        {

            var user = await _userRepository.GetByIdAsync(command.UserId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            if (!_passwordHasher.VerifyPassword(command.CurrentPassword, user.PasswordHash, user.PasswordSalt))
                throw new InvalidOperationException("Current password is incorrect");

            var (hash, salt) = _passwordHasher.HashPassword(command.NewPassword);

        
            user.ChangePassword(hash, salt);

            await _userRepository.UpdateAsync(user);

            return user.ToResponseDto();
        }


        public async Task AssignRoleAsync(Guid userId, string roleName)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found");

            var role = await _roleRepository.GetByNameAsync(roleName)
            ?? throw new InvalidOperationException($"Role '{roleName}' does not exist");

            user.AssignRole(role);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException("User not found");

            user.Delete();
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
        }


        public async Task<string> LoginAsync(LoginCommand command)
        { 
            var user = await _userRepository.GetByUserNameAsync(command.UserName);
            if (user == null)
                throw new InvalidOperationException("Invalid username or password");

            if (user.IsLocked)
                throw new InvalidOperationException("Account is locked due to multiple failed login attempts");

            if (!_passwordHasher.VerifyPassword(command.Password, user.PasswordHash, user.PasswordSalt))
            {
                user.RecordLoginFailure();
                await _userRepository.SaveChangesAsync();
                throw new InvalidOperationException("Invalid username or password");
            }

            user.RecordLoginSuccess();
            await _userRepository.SaveChangesAsync();

            return _jwtService.GenerateToken(user);
        }

        public async Task UnlockUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new InvalidOperationException("User not found");

            user.Unlock();
            await _userRepository.SaveChangesAsync();
        }

        public async Task RemoveRoleAsync(Guid userId, string roleName)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var role = user.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role != null) user.Roles.Remove(role);
            await _userRepository.SaveChangesAsync();
        }



    }
}
