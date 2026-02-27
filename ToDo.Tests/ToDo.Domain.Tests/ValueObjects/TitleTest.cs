using ToDo.Domain.Exceptions;
using ToDo.Domain.ValueObjects.Task;

namespace ToDo.Domain.Tests.ValueObjects;

public class TitleTests
{
    [Fact]
    public void Should_Create_Title_When_Value_Is_Valid()
    {
        // Arrange
        var value = "My valid title";

        // Act
        var title = new Title(value);

        // Assert
        Assert.Equal(value, title.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Should_Throw_Exception_When_Title_Is_Empty_Or_Null(string value)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new Title(value));
        Assert.Equal("Title cannot be empty.", exception.Message);
    }

    [Fact]
    public void Should_Throw_Exception_When_Title_Exceeds_100_Characters()
    {
        // Arrange
        var value = new string('a', 101);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new Title(value));
        Assert.Equal("Title cannot exceed 100 characters.", exception.Message);
    }

    [Fact]
    public void Should_Allow_Title_With_Exactly_100_Characters()
    {
        // Arrange
        var value = new string('a', 100);

        // Act
        var title = new Title(value);

        // Assert
        Assert.Equal(100, title.Value.Length);
    }
}