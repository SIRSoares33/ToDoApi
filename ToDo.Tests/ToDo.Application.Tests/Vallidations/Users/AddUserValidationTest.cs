using FluentValidation.TestHelper;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Validations.Users;

namespace ToDo.Application.Tests.Vallidations.Users;

public class AddUserValidationTests
{
    private readonly AddUserValidation _validator;

    public AddUserValidationTests()
    {
        _validator = new AddUserValidation();
    }

    [Fact]
    public void Should_Pass_Validation_For_Valid_RegisterDto()
    {
        var dto = new RegisterDto
        {
            Name = "Gustavo Soares",
            Email = "gustavo@example.com",
            Password = "Abc123!@#"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_Name_Is_Empty()
    {
        var dto = new RegisterDto { Name = "", Email = "gustavo@example.com", Password = "Abc123!@" };
        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("Name is required.");
    }

    [Fact]
    public void Should_Fail_When_Name_Is_Too_Long()
    {
        var dto = new RegisterDto
        {
            Name = new string('A', 101),
            Email = "gustavo@example.com",
            Password = "Abc123!@"
        };
        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("Name must not exceed 100 characters.");
    }

    [Fact]
    public void Should_Fail_When_Email_Is_Invalid()
    {
        var dto = new RegisterDto { Name = "Gustavo", Email = "invalid-email", Password = "Abc123!@" };
        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("Invalid email format.");
    }

    [Theory]
    [InlineData("abc123!@")]     
    [InlineData("ABC123!@")]     
    [InlineData("Abcdef!@")]     
    [InlineData("Abc123456")]    
    [InlineData("Ab1!")]          
    public void Should_Fail_When_Password_Does_Not_Meet_Criteria(string password)
    {
        var dto = new RegisterDto { Name = "Gustavo", Email = "gustavo@example.com", Password = password };
        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Fail_When_Password_Is_Empty()
    {
        var dto = new RegisterDto { Name = "Gustavo", Email = "gustavo@example.com", Password = "" };
        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("Password cannot be null or empty.");
    }
}