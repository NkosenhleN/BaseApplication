using Base.API.DTOs;
using Base.Domain.Interfaces;
using FluentValidation;

namespace Base.API.Validators
{
    public class UserCreateValidator : AbstractValidator<UserCreateDto>
    {
        private readonly IUserRepository _userRepository;

        public UserCreateValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters")
                .MustAsync(BeUniqueUserName).WithMessage("Username already exists");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email must be valid")
                .MaximumLength(100);

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(150);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .MaximumLength(150);

            RuleFor(x => x.Department)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Department));
        }

        private async Task<bool> BeUniqueUserName(string username, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            return users.All(u => !u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
