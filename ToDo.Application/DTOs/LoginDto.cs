namespace ToDo.Application.DTOs;

/// <summary>
/// Represents the data required for a user login request, including email and password credentials.
/// </summary>
/// <remarks>This data transfer object is typically used to pass authentication information from a client to a
/// server during login operations. Ensure that sensitive information, such as the password, is handled securely and
/// transmitted over encrypted channels.</remarks>
public class LoginDto
{
    /// <summary>
    /// Gets the email address associated with the user.
    /// </summary>
    public string Email { get; set; } = null!;
    /// <summary>
    /// Gets the password used for authentication.
    /// </summary>
    public string Password { get; set; } = null!;
}