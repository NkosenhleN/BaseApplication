using Base.Application.Commands;
using Base.Application.Interfaces;
using Base.Application.Mappings;
using Base.Application.Responses;
using Base.Domain.Entities;
using Base.Domain.Interfaces;

namespace Base.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserResponse> CreateUserAsync(CreateUserCommand command)
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

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {

            var users = await _userRepository.GetAllAsync();

            return users.Select(u => u.ToResponseDto()).ToList();
        }

        public async Task<UserResponse?> GetByIdAsync(Guid id)
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


        public async Task<UserResponse> ChangePasswordAsync(ChangePasswordCommand command)
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

    }
}
