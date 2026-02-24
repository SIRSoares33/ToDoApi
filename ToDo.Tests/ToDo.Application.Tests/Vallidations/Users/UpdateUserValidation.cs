using FluentValidation.TestHelper;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Validations.Users;

namespace ToDo.Application.Tests.Vallidations.Users;

public class UpdateUserValidationTests
{
    private readonly UpdateUserValidation _validator;

    public UpdateUserValidationTests()
    {
        _validator = new UpdateUserValidation();
    }

    [Fact]
    public void Should_Pass_When_All_Fields_Are_Empty()
    {
        var dto = new UpdateUserDto
        {
            Name = "",
            Email = "",
            Password = ""
        };

        var result = _validator.TestValidate(dto);

        // Nenhuma validação deve disparar
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_Name_Is_Too_Long()
    {
        var dto = new UpdateUserDto
        {
            Name = new string('A', 101)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("Name must not exceed 100 characters.");
    }

    [Fact]
    public void Should_Fail_When_Email_Is_Invalid()
    {
        var dto = new UpdateUserDto
        {
            Email = "invalid-email"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Email)
              .WithErrorMessage("Invalid email format.");
    }

    [Theory]
    [InlineData("abc123!@")]   // sem maiúscula
    [InlineData("ABC123!@")]   // sem minúscula
    [InlineData("Abcdef!@")]   // sem dígito
    [InlineData("Abc123456")]  // sem caractere especial
    [InlineData("Ab1!")]       // menor que 8 caracteres
    public void Should_Fail_When_Password_Does_Not_Meet_Criteria(string password)
    {
        var dto = new UpdateUserDto
        {
            Password = password
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Pass_When_All_Fields_Are_Valid()
    {
        var dto = new UpdateUserDto
        {
            Name = "Gustavo Soares",
            Email = "gustavo@example.com",
            Password = "Abc123!@"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }
}