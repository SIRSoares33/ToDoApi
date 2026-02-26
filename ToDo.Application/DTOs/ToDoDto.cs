namespace ToDo.Application.DTOs;

public class ToDoDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public string CreatedAt { get; set; } = null!;
    public string UpdatedAt { get; set; } = null!;
}
