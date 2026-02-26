using ToDo.Domain.Exceptions;
using ToDo.Domain.ValueObjects;
using ToDo.Domain.ValueObjects.Users;

namespace ToDo.Domain.Tests.ValueObjects;

public class PasswordTest
{
    [Fact]
    public void Create_ValidPassword_ShouldSucceed()
    {
        // Arrange
        var validPassword = "P@ssw0rd!";
        // Act
        var password = new Password(validPassword);
        // Assert
        Assert.Equal(validPassword, password.Value);
    }

    [Fact]
    public void Create_PasswordWithLeadingAndTrailingSpaces_ShouldTrim()
    {
        // Arrange
        var passwordWithSpaces = "  P@ssw0rd!  ";
        var expectedPassword = "P@ssw0rd!";
        // Act
        var password = new Password(passwordWithSpaces);
        // Assert
        Assert.Equal(expectedPassword, password.Value);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("short")]
    [InlineData("nouppercase1!")]
    [InlineData("NOLOWERCASE1!")]
    [InlineData("NoDigit!")]
    [InlineData("NoSpecialChar1")]
    public void Create_PasswordWithoutUppercase_ShouldThrowDomainException(string? invalidPassword)
        => Assert.Throws<DomainException>(() => new Password(invalidPassword));
}
