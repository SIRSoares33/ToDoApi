using MediatR;
using ToDo.Application.Features.Commands.Auth;
using ToDo.Application.Features.Responses.Auth;
using ToDo.Application.Interfaces;

namespace ToDo.Application.Features.Handlers.Auth;

/// <summary>
/// Handles user login requests by validating credentials and generating a JWT access token.
/// </summary>
/// <remarks>This handler is typically used in authentication workflows to process login commands. It validates
/// the provided email and password, and issues a JWT token upon successful authentication. Exceptions are thrown if the
/// email is not found or the password is invalid.</remarks>
/// <param name="repository">The user repository used to retrieve user information based on email address.</param>
/// <param name="jwtService">The JWT token service used to generate access tokens for authenticated users.</param>
/// <param name="hasher">The password hasher used to verify user passwords against stored hashes.</param>
public class LoginHandler(IAuthService authService) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        => await authService.LoginAsync(request.LoginDto, cancellationToken);
}