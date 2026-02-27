using ToDo.Domain.Exceptions;
using ToDo.Domain.ValueObjects.Task;

namespace ToDo.Domain.Tests.ValueObjects;

public class DescriptionTests
{
    [Fact]
    public void Should_Create_Description_When_Value_Is_Valid()
    {
        // Arrange
        var value = "This is a valid description";

        // Act
        var description = new Description(value);

        // Assert
        Assert.Equal(value, description.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Allow_Null_Or_Empty_Description(string? value)
    {
        // Act
        var description = new Description(value);

        // Assert
        Assert.Null(description.Value);
    }

    [Fact]
    public void Should_Throw_Exception_When_Description_Exceeds_500_Characters()
    {
        // Arrange
        var value = new string('a', 501);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new Description(value));
        Assert.Equal("Description cannot exceed 500 characters.", exception.Message);
    }

    [Fact]
    public void Should_Allow_Description_With_Exactly_500_Characters()
    {
        // Arrange
        var value = new string('a', 500);

        // Act
        var description = new Description(value);

        // Assert
        Assert.Equal(500, description.Value!.Length);
    }
}