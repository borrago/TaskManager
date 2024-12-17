using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Requests;
using TaskManager.Application.Commands.AddTaskCommand;
using TaskManager.Application.Commands.DeleteTaskCommand;
using TaskManager.Application.Commands.UpdateTaskCommand;
using TaskManager.Application.Queries.GetTaskByIdQuery;
using TaskManager.Application.Queries.GetTaskPerformanceQuery;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet("performance")]
    //[Authorize(Roles = "Manager")]
    public async Task<ActionResult<GetTaskPerformanceQueryResult>> Get(CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetTaskPerformanceQueryInput(), cancellationToken));
}