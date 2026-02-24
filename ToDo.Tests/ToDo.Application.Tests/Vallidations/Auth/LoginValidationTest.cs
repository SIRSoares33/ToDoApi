using FluentValidation.TestHelper;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Validations.Auth;

namespace ToDo.Application.Tests.Vallidations.Auth;

public class LoginValidationTests
{
    private readonly LoginValidation _validator;

    public LoginValidationTests() => _validator = new LoginValidation();

    [Fact]
    public void Should_Pass_Validation_For_Valid_LoginDto()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "gustavo@example.com",
            Password = "Password123"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_Email_Is_Empty()
    {
        var dto = new LoginDto
        {
            Email = "",
            Password = "Password123"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is required.");
    }

    [Fact]
    public void Should_Fail_When_Email_Is_Invalid()
    {
        var dto = new LoginDto
        {
            Email = "invalid-email",
            Password = "Password123"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email format.");
    }

    [Fact]
    public void Should_Fail_When_Password_Is_Empty()
    {
        var dto = new LoginDto
        {
            Email = "gustavo@example.com",
            Password = ""
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password is required.");
    }

    [Fact]
    public void Should_Fail_When_Password_Is_Too_Short()
    {
        var dto = new LoginDto
        {
            Email = "gustavo@example.com",
            Password = "1234567"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password must be at least 8 characters long.");
    }
}