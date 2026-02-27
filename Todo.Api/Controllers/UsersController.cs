using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Commands.Users;
using ToDo.Application.Features.Queries.Users;
using ToDo.Application.Interfaces;

namespace ToDo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IMediator mediator, IUserClaims userClaims) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken) 
        => Ok(await mediator.Send(new GetUsersQuery(), cancellationToken));

    [Authorize(Roles = "Admin")] 
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
    { await mediator.Send(new UpdateUserByAdminCommand(id, dto), cancellationToken); return NoContent(); }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateOwnAccount([FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
    {
        await mediator.Send(new UpdateUserCommand(userClaims.UserId, dto), cancellationToken);
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
        await mediator.Send(new DeleteUserCommand(userClaims.UserId), cancellationToken);
        return NoContent();
    }
}