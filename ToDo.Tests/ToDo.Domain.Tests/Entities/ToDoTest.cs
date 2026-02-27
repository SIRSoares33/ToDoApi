using ToDo.Domain.Entities;
using ToDo.Domain.ValueObjects.Task;

namespace ToDo.Domain.Tests.Entities;

public class TodoTests
{
    private static Todo CreateTodo()
    {
        return new Todo(
            new Title("Initial title"),
            new Description("Initial description"),
            false,
            Guid.NewGuid()
        );
    }

    [Fact]
    public void Should_Create_Todo_Correctly()
    {
        // Arrange
        var title = new Title("Test title");
        var description = new Description("Test description");
        var userId = Guid.NewGuid();

        // Act
        var todo = new Todo(title, description, false, userId);

        // Assert
        Assert.NotEqual(Guid.Empty, todo.Id);
        Assert.Equal(title, todo.Title);
        Assert.Equal(description, todo.Description);
        Assert.False(todo.IsCompleted);
        Assert.Equal(userId, todo.UserId);
        Assert.Equal(todo.CreateAt, todo.UpdateAt);
    }

    [Fact]
    public void Should_Update_Title_When_Title_Is_Different()
    {
        var todo = CreateTodo();
        var oldUpdateAt = todo.UpdateAt;

        Thread.Sleep(10);

        todo.Update("New title", null, null);

        Assert.Equal("New title", todo.Title.Value);
        Assert.True(todo.UpdateAt > oldUpdateAt);
    }

    [Fact]
    public void Should_Update_Description_When_Description_Is_Different()
    {
        var todo = CreateTodo();
        var oldUpdateAt = todo.UpdateAt;

        Thread.Sleep(10);

        todo.Update(null, "New description", null);

        Assert.Equal("New description", todo.Description.Value);
        Assert.True(todo.UpdateAt > oldUpdateAt);
    }

    [Fact]
    public void Should_Update_IsCompleted_When_Value_Is_Different()
    {
        var todo = CreateTodo();
        var oldUpdateAt = todo.UpdateAt;

        Thread.Sleep(10);

        todo.Update(null, null, true);

        Assert.True(todo.IsCompleted);
        Assert.True(todo.UpdateAt > oldUpdateAt);
    }

    [Fact]
    public void Should_Update_Multiple_Fields_At_Once()
    {
        var todo = CreateTodo();

        todo.Update("Updated title", "Updated description", true);

        Assert.Equal("Updated title", todo.Title.Value);
        Assert.Equal("Updated description", todo.Description.Value);
        Assert.True(todo.IsCompleted);
    }

    [Fact]
    public void Should_Not_Update_When_Values_Are_The_Same()
    {
        var todo = CreateTodo();
        var updateAt = todo.UpdateAt;

        Thread.Sleep(10);

        todo.Update("Initial title", "Initial description", false);

        Assert.Equal(updateAt, todo.UpdateAt);
    }

    [Fact]
    public void Should_Not_Update_When_All_Values_Are_Null()
    {
        var todo = CreateTodo();
        var updateAt = todo.UpdateAt;

        Thread.Sleep(10);

        todo.Update(null, null, null);

        Assert.Equal(updateAt, todo.UpdateAt);
    }

    [Fact]
    public void Should_Not_Update_When_Title_And_Description_Are_Whitespace()
    {
        var todo = CreateTodo();
        var updateAt = todo.UpdateAt;

        Thread.Sleep(10);

        todo.Update(" ", " ", null);

        Assert.Equal(updateAt, todo.UpdateAt);
    }
}