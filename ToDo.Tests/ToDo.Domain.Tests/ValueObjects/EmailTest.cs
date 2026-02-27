using ToDo.Domain.Exceptions;
using ToDo.Domain.ValueObjects.Users;

namespace ToDo.Domain.Tests.ValueObjects;

public class EmailTest
{
    [Fact]
    public void Constructor_Should_Create_Email_When_Valid()
    {
        // Arrange
        var validEmail = "email@email.com";
        // Act
        var email = new Email(validEmail);
        // Assert
        Assert.Equal(validEmail, email.Value);
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("invalid-email")]
    [InlineData("  email@email.com  ")]
    public void Constructor_Should_Throw_DomainException_When_Invalid(string? invalidEmail)
    => Assert.Throws<DomainException>(() => new Email(invalidEmail));
}
