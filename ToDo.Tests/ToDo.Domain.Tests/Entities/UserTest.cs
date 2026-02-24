using ToDo.Domain.Entities;
using ToDo.Domain.Enums;
using ToDo.Domain.Exceptions;
using ToDo.Domain.ValueObjects;

namespace ToDo.Domain.Tests.Entities;

public class UserTest
{
    [Fact]
    public void Create_ValidUser_ShouldSucceed()
    {
        // Arrange
        var name = new Name("John Doe");
        var email = new Email("email@email.com");
        var password = new Password("Password123!");

        // Act
        var user = new User(name, email, password);

        // Assert   
        Assert.NotNull(user);
        Assert.Equal(name, user.Name);
        Assert.Equal(email, user.Email);
        Assert.Equal(password, user.HashPassword);
    }

    [Fact]
    public void Update_User_ShouldSucceed()
    {
        // Arrange
        var user = new User(new Name("John Doe"), new Email("email@email.com"), new Password("Password123!"), Role.User);

        // Act
        user.Update("Jane Doe Updated", "email@emailUpdated", "Password123!Updated", Role.Admin);

        // Assert
        Assert.Equal("Jane Doe Updated", user.Name.Value);
        Assert.Equal("email@emailUpdated", user.Email.Value);
        Assert.Equal("Password123!Updated", user.HashPassword.Value);
        Assert.Equal(Role.Admin, user.Role);
    }

    [Theory]
    [InlineData(null, null, null, null)]
    [InlineData("", "", "", null)]
    public void Update_UserWithEmptyData_ShouldDoNothing(string? name, string? email, string? password, Role? role)
    {
        // Arrange
        var user = new User(new Name("John Doe"), new Email("email@email.com"), new Password("Password123!"), Role.User);

        // Act
        user.Update(name, email, password, role);

        // Assert
        Assert.Equal("John Doe", user.Name.Value);
        Assert.Equal("email@email.com", user.Email.Value);
        Assert.Equal("Password123!", user.HashPassword.Value);
        Assert.Equal(Role.User, user.Role);
    }

    [Theory]
    [InlineData(null, "  ", "   ", null)]
    [InlineData(" ", " ", " ", null)]
    public void Update_UserWithWhitespaceData_ShouldThrowDomainException(string? name, string? email, string? password, Role? role)
    {
        // Arrange
        var user = new User(new Name("John Doe"), new Email("email@email.com"), new Password("Password123!"), Role.User);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => user.Update(name, email, password, role));
    }
}