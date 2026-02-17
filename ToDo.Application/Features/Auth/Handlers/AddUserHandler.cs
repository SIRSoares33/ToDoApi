using AutoMapper;
using MediatR;
using ToDo.Application.Features.Auth.Commands;
using ToDo.Application.Interfaces;
using ToDo.Domain.Entities;

namespace ToDo.Application.Features.Auth.Handlers;

/// <summary>
/// Handles the addition of a new user by processing an <see cref="AddUserCommand"/> request.
/// </summary>
/// <remarks>This handler delegates user registration to the provided authentication service and assigns the
/// standard user role. It is typically used within a request processing pipeline to encapsulate user creation
/// logic.</remarks>
/// <param name="service">The authentication service used to register new users.</param>
public class AddUserHandler(IAuthService service, IMapper mapper) : IRequestHandler<AddUserCommand, Unit>
{
    public async Task<Unit> Handle(AddUserCommand command, CancellationToken cancellationToken)
        => await service.RegisterAsync(mapper.Map<User>(command.Dto), command.Role, cancellationToken);
}