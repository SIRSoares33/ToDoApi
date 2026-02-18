using ToDo.Domain.Enums;

namespace ToDo.Application.DTOs;

/// <summary>
/// Represents the data required to update an existing user's profile information.
/// </summary>
/// <remarks>This data transfer object is typically used in user management operations to modify user details such
/// as name, email, password, and role. All properties should be set to valid values before submitting an update
/// request. The role property determines the user's access level within the system.</remarks>
public class UpdateUserDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public Role? Role { get; set; }
}