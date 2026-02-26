using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.DTOs;
using ToDo.Application.Features.Commands.Task;
using ToDo.Application.Features.Commands.ToDo;
using ToDo.Application.Features.Queries.Task;

namespace Todo.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TasksController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] AddToDoDto dto, CancellationToken cancellationToken)
    {
        await mediator.Send(new AddToDoCommand(dto), cancellationToken);
        return Created();
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks(CancellationToken cancellationToken)
    {
        var tasks = await mediator.Send(new GetToDosQuery(), cancellationToken);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(Guid id, CancellationToken cancellationToken)
    {
        var task = await mediator.Send(new GetToDoByIdQuery(id), cancellationToken);
        return Ok(task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateToDoDto dto, CancellationToken cancellationToken)
    {
        await mediator.Send(new UpdateToDoCommand(id, dto), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id, CancellationToken cancellationToken)
    { 
        await mediator.Send(new RemoveToDoCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAllTasks(CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveToDoCommand(null), cancellationToken);
        return NoContent();
    }
}