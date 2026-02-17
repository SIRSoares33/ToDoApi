using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Auth.Commands;
using ToDo.Application.Features.Auth.Queries;

namespace Todo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken) 
        => Ok(await mediator.Send(new GetUsersQuery(), cancellationToken));

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
    { await mediator.Send(new UpdateUserCommand(id, dto), cancellationToken); return NoContent(); }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateOwnAccount([FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var guidId))
            return Unauthorized();

        dto.Role = null; // Prevent users from changing their own role.

        await mediator.Send(new UpdateUserCommand(guidId, dto), cancellationToken);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    { await mediator.Send(new DeleteUserCommand(id), cancellationToken); return NoContent(); }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteOwnAccount(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var guidId))
            return Unauthorized();

        await mediator.Send(new DeleteUserCommand(guidId), cancellationToken);
        return NoContent();
    }
}