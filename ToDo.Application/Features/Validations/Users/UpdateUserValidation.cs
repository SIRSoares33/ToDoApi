using FluentValidation;
using ToDo.Application.DTOs;

namespace ToDo.Application.Features.Validations.Users;

/// <summary>
///  validation class for updating a user's information,
///  ensuring that the provided data adheres to specific rules and constraints.
/// </summary>
public class UpdateUserValidation : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidation()
    {

        When(x => !string.IsNullOrEmpty(x.Name), () =>
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters."); 
        });

        When(x => !string.IsNullOrEmpty(x.Email), () =>
        {
            RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format.");
        });

        When(x => !string.IsNullOrEmpty(x.Password), () =>
        {
            RuleFor(x => x.Password)
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
        });
    }
}