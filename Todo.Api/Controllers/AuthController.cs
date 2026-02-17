using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Auth.Commands;
using ToDo.Domain.Enums;

namespace Todo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken) 
        => Ok(await mediator.Send(new LoginCommand(dto), cancellationToken));

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto, CancellationToken cancellationToken)
    { await mediator.Send(new AddUserCommand(dto, Role.User), cancellationToken); return Created(); }

    [Authorize(Roles = "Admin")]
    [HttpPost("register/admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto dto, CancellationToken cancellationToken)
    { await mediator.Send(new AddUserCommand(dto, Role.Admin), cancellationToken); return Created(); }
}