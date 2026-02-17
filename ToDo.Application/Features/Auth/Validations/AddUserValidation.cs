using FluentValidation;
using ToDo.Application.DTOs;

namespace ToDo.Application.Features.Auth.Validations;

public class AddUserValidation : AbstractValidator<RegisterDto>
{
    public AddUserValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithMessage("Password cannot be null or empty.")
            .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
            .Must(p => p.Any(char.IsUpper))
                .WithMessage("Password must contain at least one uppercase letter.")
            .Must(p => p.Any(char.IsLower))
                .WithMessage("Password must contain at least one lowercase letter.")
            .Must(p => p.Any(char.IsDigit))
                .WithMessage("Password must contain at least one digit.")
            .Must(p => p.Any(ch => !char.IsLetterOrDigit(ch)))
                .WithMessage("Password must contain at least one special character.");
    }
}
