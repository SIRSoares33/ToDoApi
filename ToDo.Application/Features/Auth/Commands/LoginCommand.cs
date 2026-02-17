using MediatR;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Auth.Responses;

namespace ToDo.Application.Features.Auth.Commands;

/// <summary>
/// Represents a request to perform a login operation using the specified credentials.
/// </summary>
/// <param name="LoginDto">The login credentials and related information to be used for authentication. Cannot be null.</param>
public record LoginCommand(LoginDto LoginDto) : IRequest<LoginResponse>;