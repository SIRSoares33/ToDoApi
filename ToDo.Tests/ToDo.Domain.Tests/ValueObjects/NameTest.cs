using ToDo.Domain.Exceptions;
using ToDo.Domain.ValueObjects;

namespace ToDo.Domain.Tests.ValueObjects;

public class NameTest
{
    [Fact]
    public void Create_ValidName_ShouldSucceed()
    {
        // Arrange
        var validName = "John Doe";
        // Act
        var name = new Name(validName);
        // Assert
        Assert.NotNull(name);
        Assert.Equal(validName, name.Value);
    }

    [Fact]
    public void Create_NameWithLeadingAndTrailingSpaces_ShouldTrim()
    {
        // Arrange
        var nameWithSpaces = "  John Doe  ";
        var expectedName = "John Doe";
        // Act
        var name = new Name(nameWithSpaces);
        // Assert
        Assert.NotNull(name);
        Assert.Equal(expectedName, name.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Jo")]
    [InlineData("John123")]
    [InlineData("John_Doe!")]
    [InlineData("John\u0000Doe")]
    public void Create_InvalidName_ShouldThrowDomainException(string? invalidName)
        => Assert.Throws<DomainException>(() => new Name(invalidName));
    
}