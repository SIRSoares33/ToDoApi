namespace ToDo.Application.Interfaces;

/// <summary>
/// this interface defines the contract for accessing user claims such as user ID, role, email, and username.
/// It is designed to be implemented by classes that retrieve this information from the current user's claims, 
/// typically in a web application context where user authentication and authorization are handled via claims-based identity.
/// </summary>
public interface IUserClaims
{
    /// <summary>
    /// Gets the unique identifier of the current user. 
    /// This is typically retrieved from the user's claims, often using the ClaimTypes.NameIdentifier claim type. 
    /// The value is expected to be a GUID string that can be parsed into a Guid object. 
    /// If the claim is not found or cannot be parsed, an exception will be thrown.
    /// </summary>
    Guid UserId { get; }
    /// <summary>
    /// gets the role of the current user. This is typically retrieved from the user's claims, often using the ClaimTypes.
    /// Role claim type.
    /// </summary>
    string UserRole { get; }
    /// <summary>
    /// gets the email address of the current user. 
    /// This is typically retrieved from the user's claims, often using the ClaimTypes.Email claim type.
    /// </summary>
    string Email { get; }
    /// <summary>
    /// gets the username of the current user. 
    /// This is typically retrieved from the user's claims, often using the ClaimTypes.Name claim type.
    /// </summary>
    string UserName { get; }
}