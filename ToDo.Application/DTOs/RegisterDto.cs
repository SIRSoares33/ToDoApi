namespace ToDo.Application.DTOs;

/// <summary>
/// Represents the data required to register a new user account, including name, email address, and password.
/// </summary>
/// <remarks>This data transfer object is typically used when submitting registration information to an
/// authentication or user management service. All properties must be provided with valid values to successfully create
/// a new user.</remarks>
public class RegisterDto
{
    /// <summary>
    /// Gets or sets the name associated with the object.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the email address associated with the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the password used for authentication.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}