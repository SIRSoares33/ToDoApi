using FluentValidation.TestHelper;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Validations.ToDo;

namespace ToDo.Application.Tests.Vallidations.ToDo;

public class UpdateToDoValidationTests
{
    private readonly UpdateToDoValidation _validator;

    public UpdateToDoValidationTests() => _validator = new UpdateToDoValidation();

    [Fact]
    public void Should_Not_Have_Error_When_Title_Is_Null()
    {
        var dto = new UpdateToDoDto
        {
            Title = null,
            Description = "Valid description"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Title_Is_Empty()
    {
        var dto = new UpdateToDoDto
        {
            Title = string.Empty,
            Description = "Valid description"
        };

        var result = _validator.TestValidate(dto);

        // Regra não dispara por causa do When
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Title_Exceeds_Max_Length()
    {
        var dto = new UpdateToDoDto
        {
            Title = new string('A', 101),
            Description = "Valid description"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Title)
              .WithErrorMessage("Title must not exceed 100 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Whitespace()
    {
        var dto = new UpdateToDoDto
        {
            Title = "   ",
            Description = "Valid description"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Title)
              .WithErrorMessage("Title is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_Max_Length()
    {
        var dto = new UpdateToDoDto
        {
            Title = "Valid title",
            Description = new string('B', 501)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Description)
              .WithErrorMessage("Description must not exceed 500 characters.");
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Dto_Is_Valid()
    {
        var dto = new UpdateToDoDto
        {
            Title = "Valid title",
            Description = "Valid description"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }
}