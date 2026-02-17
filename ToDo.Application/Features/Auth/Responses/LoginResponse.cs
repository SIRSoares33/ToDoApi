namespace ToDo.Application.Features.Auth.Responses;

/// <summary>
/// Represents the result of a login operation, including the user's unique identifier and authentication token.
/// </summary>
/// <param name="Id">The unique identifier assigned to the authenticated user.</param>
/// <param name="Token">The authentication token issued upon successful login. This token is used to authorize subsequent requests.</param>
public record LoginResponse(Guid Id, string Token);