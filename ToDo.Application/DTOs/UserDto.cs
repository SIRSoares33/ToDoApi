using ToDo.Domain.Enums;

namespace ToDo.Application.DTOs;

/// <summary>
/// Represents the data transfer object for user information, including the user's unique identifier, name, email, and role.
/// </summary>
public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
}
