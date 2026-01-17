using Base.API.DTOs;
using Base.Domain.Interfaces;
using FluentValidation;

namespace Base.API.Validators
{
    public class UserCreateValidator : AbstractValidator<CreateUserDto>
    {
        private readonly IUserRepository _userRepository;

        public UserCreateValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters")
                .MustAsync(async (username, _) =>
                !await userRepository.UsernameExistsAsync(username)).WithMessage("Username already exists");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email must be valid")
                .MaximumLength(100);

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(20);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .MaximumLength(20);

            RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6).WithMessage("New password must be at least 6 characters");
        //.Matches("[A-Z]").WithMessage("Password must contain an uppercase letter")
        //.Matches("[a-z]").WithMessage("Password must contain a lowercase letter")
        //.Matches("[0-9]").WithMessage("Password must contain a number")
        //.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain a special character");

    }
    }
}
