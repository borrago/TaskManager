using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Requests;
using TaskManager.Application.Commands.AddTaskCommand;
using TaskManager.Application.Commands.DeleteTaskCommand;
using TaskManager.Application.Commands.UpdateTaskCommand;
using TaskManager.Application.Queries.GetTaskByIdQuery;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var command = new AddTaskCommandInput(request.ProjectId, request.AssignedUserId, request.Title, request.Description, request.EndDate, request.Status, request.Priority);
        var client = await _mediator.Send(command, cancellationToken);

        if (client.Id == Guid.Empty)
            return BadRequest("Erro ao cadastrar o tarefa.");

        return CreatedAtAction(nameof(Create), client);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateTaskCommandInput(id, request.Title, request.Description, request.EndDate, request.Status);
        var client = await _mediator.Send(command, cancellationToken);

        if (client.Id == Guid.Empty)
            return BadRequest($"Erro ao atualizar o tarefa {command.Id}.");

        return Ok(client);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var client = await _mediator.Send(new DeleteTaskCommandInput(id), cancellationToken);

        if (client.Id == Guid.Empty)
            return BadRequest($"Erro ao excluir o tarefa {id}.");

        return Ok(client);
    }

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<GetTaskByIdQueryResult>> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetTaskByIdQueryInput(id), cancellationToken));
}