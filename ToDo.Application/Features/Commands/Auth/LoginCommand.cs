using MediatR;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Responses.Auth;

namespace ToDo.Application.Features.Commands.Auth;

/// <summary>
/// Represents a request to perform a login operation using the specified credentials.
/// </summary>
/// <param name="LoginDto">The login credentials and related information to be used for authentication. Cannot be null.</param>
public record LoginCommand(LoginDto LoginDto) : IRequest<LoginResponse>;