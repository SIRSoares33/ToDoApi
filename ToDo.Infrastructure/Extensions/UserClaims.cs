using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ToDo.Application.Interfaces;

namespace ToDo.Infrastructure.Extensions;

/// <summary>
/// Provides access to the current user's claim-based identity information, including user ID, email and role, from
/// the HTTP context.   
/// </summary>
/// <remarks>This class retrieves claim values from the current HTTP context's user principal. It is intended for
/// use in web applications where user claims are available via the HTTP context. Accessing these properties when no
/// HTTP context is available, or when the expected claims are missing, will result in an exception.</remarks>
public class UserClaims(IHttpContextAccessor http) : IUserClaims
{
    public Guid UserId => Guid.Parse(http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? throw new UnauthorizedAccessException("User id not founded."));
    public string UserRole => http.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value
        ?? throw new KeyNotFoundException("User role not founded.");
    public string Email => http.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value
        ?? throw new UnauthorizedAccessException("User email not founded.");
    public string UserName => http.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value
        ?? throw new KeyNotFoundException("User name not founded.");
}