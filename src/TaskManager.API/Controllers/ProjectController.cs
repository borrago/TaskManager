using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Requests;
using TaskManager.Application.Commands.AddProjectCommand;
using TaskManager.Application.Commands.DeleteProjectCommand;
using TaskManager.Application.Commands.UpdateProjectCommand;
using TaskManager.Application.Queries.GetProjectByIdQuery;
using TaskManager.Application.Queries.GetProjectQuery;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var command = new AddProjectCommandInput(request.Name, request.Description, request.UserId);
        var client = await _mediator.Send(command, cancellationToken);

        if (client.Id == Guid.Empty)
            return BadRequest("Erro ao cadastrar o projeto.");

        return CreatedAtAction(nameof(Create), client);
    }

    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateProjectCommandInput(id, request.Name, request.Description);
        var client = await _mediator.Send(command, cancellationToken);

        if (client.Id == Guid.Empty)
            return BadRequest($"Erro ao atualizar o projeto {command.Id}.");

        return Ok(client);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var client = await _mediator.Send(new DeleteProjectCommandInput(id), cancellationToken);

        if (client.Id == Guid.Empty)
            return BadRequest($"Erro ao excluir o projeto {id}.");

        return Ok(client);
    }

    [HttpGet]
    public async Task<ActionResult<GetProjectQueryResult>> Get(CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetProjectQueryInput(), cancellationToken));

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<GetProjectByIdQueryResult>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetProjectByIdQueryInput(id), cancellationToken));
}