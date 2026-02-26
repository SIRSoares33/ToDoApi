using System.ComponentModel.DataAnnotations;
using ToDo.Domain.ValueObjects.Task;

namespace ToDo.Domain.Entities;

public class Todo
{
    #region Properties
    [Key]
    public Guid Id { get; private set; }
    public Title Title { get; private set; } = null!;
    public Description Description { get; private set; } = null!;
    public bool IsCompleted { get; private set; } = false;
    public DateTime CreateAt { get; private set; }
    public DateTime UpdateAt { get; private set; }
    public Guid UserId { get; set; }
    #endregion

    #region Constructors
    public Todo() { }

    public Todo(Title title, Description description, bool isCompleted, Guid userId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        IsCompleted = isCompleted;

        CreateAt = DateTime.UtcNow;
        UpdateAt = CreateAt;
        UserId = userId;
    }
    #endregion

    #region Methods
    public void Update(string? title, string? description, bool? isCompleted)
    {
        var changed = UpdateTitle(title) | 
            UpdateDescription(description) | 
            UpdateCompleted(isCompleted);

        if (changed) Touch();
    }

    private bool UpdateTitle(string? title)
    {
        if (string.IsNullOrWhiteSpace(title) || title == Title.Value) return false;

        Title = new Title(title);

        return true;
    }
    private bool UpdateDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description) || description == Description.Value) return false;

        Description = new Description(description);

        return true;
    }

    private bool UpdateCompleted(bool? isCompleted)
    {
        if (!isCompleted.HasValue || IsCompleted == isCompleted)
            return false;

        IsCompleted = isCompleted.Value;
        return true;
    }

    private void Touch() 
        => UpdateAt = DateTime.UtcNow;
    #endregion
}
