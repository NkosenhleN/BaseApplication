using Base.API.DTOs;
using Base.Domain.Interfaces;
using FluentValidation;
namespace Base.API.Validators;
public class UpdatePasswordDtoValidator: AbstractValidator<ChangePasswordDto>
{
        public UpdatePasswordDtoValidator()
        {
            RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required")
                .MinimumLength(6).WithMessage("New password must be at least 6 characters");
        }
}
