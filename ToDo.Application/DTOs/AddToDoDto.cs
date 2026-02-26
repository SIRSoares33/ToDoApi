namespace ToDo.Application.DTOs;

public class AddToDoDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
}