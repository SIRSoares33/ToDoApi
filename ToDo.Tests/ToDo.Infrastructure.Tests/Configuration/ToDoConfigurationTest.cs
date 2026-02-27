using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;
using ToDo.Infrastructure.Tests.Configuration.Helper;

namespace ToDo.Infrastructure.Tests.Configuration;

public class ToDoConfigurationTests
{
    private readonly TestDbContext _context = ModelHelper.CreateContext();

    [Fact]
    public void Should_Map_To_Correct_Table_Name()
    {
        var entity = _context.Model.FindEntityType(typeof(Todo));

        Assert.Equal("ToDos", entity!.GetTableName());
    }

    [Fact]
    public void Should_Have_Primary_Key_Id()
    {
        var entity = _context.Model.FindEntityType(typeof(Todo));
        var key = entity!.FindPrimaryKey();

        Assert.Single(key!.Properties);
        Assert.Equal("Id", key.Properties.First().Name);
    }

    [Fact]
    public void Should_Map_Title_As_Owned_With_Required_Value()
    {
        var todoEntity = _context.Model.FindEntityType(typeof(Todo))!;

        var titleEntity = _context.Model.GetEntityTypes()
            .First(e => e.ClrType == typeof(ToDo.Domain.ValueObjects.Task.Title));

        var valueProperty = titleEntity
            .FindProperty(nameof(ToDo.Domain.ValueObjects.Task.Title.Value))!;

        Assert.Equal("Title", valueProperty.GetColumnName());
        Assert.False(valueProperty.IsNullable);
    }

    [Fact]
    public void Should_Map_Description_As_Owned_And_Optional()
    {
        var descriptionEntity = _context.Model.GetEntityTypes()
            .First(e => e.ClrType == typeof(ToDo.Domain.ValueObjects.Task.Description));

        var valueProperty = descriptionEntity
            .FindProperty(nameof(ToDo.Domain.ValueObjects.Task.Description.Value))!;

        Assert.Equal("Description", valueProperty.GetColumnName());
        Assert.True(valueProperty.IsNullable);
    }

    [Fact]
    public void Should_Require_IsCompleted_CreateAt_UpdateAt_And_UserId()
    {
        var entity = _context.Model.FindEntityType(typeof(Todo))!;

        Assert.False(entity.FindProperty("IsCompleted")!.IsNullable);
        Assert.False(entity.FindProperty("CreateAt")!.IsNullable);
        Assert.False(entity.FindProperty("UpdateAt")!.IsNullable);
        Assert.False(entity.FindProperty("UserId")!.IsNullable);
    }
}