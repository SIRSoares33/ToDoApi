using FluentValidation;
using ToDo.Application.DTOs;

namespace ToDo.Application.Features.Validations.ToDo;

public class UpdateToDoValidation : AbstractValidator<UpdateToDoDto>
{
    public UpdateToDoValidation()
    {
        When(x => !string.IsNullOrEmpty(x.Title), () =>
        {
            RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        });
        
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
