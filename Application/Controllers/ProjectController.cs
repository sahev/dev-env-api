using Application.Handlers.ProjectsHandler.Command;
using Application.Handlers.ProjectsHandler.Query;
using Domain.Dtos.Project;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Controllers;

public class ProjectController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// get a Project by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [SwaggerResponse(200, "Project details retrieved successfully.", typeof(ProjectDto))]
    [SwaggerResponse(404, "Project not found.")]
    public async Task<IActionResult> Get(string id)
    {
        var response = await _mediator.Send((GetProjectQuery)id);

        return Ok(response);
    }

    /// <summary>
    /// get a list of Projects
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(200, "Projects retrieved successfully.", typeof(Pagination<ProjectDto>))]
    public async Task<IActionResult> Get(int pageIndex = 0, int pageSize = 10)
    {
        var response = await _mediator.Send(new GetAllProjectsQuery { PageIndex = pageIndex, PageSize = pageSize });

        return Ok(response);
    }

    /// <summary>
    /// add a Project
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse(201, "Project added successfully.", typeof(ProjectDto))]
    [SwaggerResponse(400, "Invalid request.")]
    public async Task<IActionResult> Add(AddProjectCommand request, CancellationToken token)
        => Ok(await _mediator.Send(request, token));

    /// <summary>
    /// update a Project
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPut]
    [SwaggerResponse(200, "Project updated successfully.", typeof(ProjectDto))]
    [SwaggerResponse(400, "Invalid request.")]
    [SwaggerResponse(404, "Project not found.")]
    public async Task<IActionResult> Update(PutProjectCommand request, CancellationToken token)
        => Ok(await _mediator.Send(request, token));

    /// <summary>
    /// delete a Project by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [SwaggerResponse(200, "Project deleted successfully.")]
    [SwaggerResponse(404, "Project not found.")]
    public async Task<IActionResult> Delete(string id, CancellationToken token)
    {
        await _mediator.Send((DeleteProjectCommand)id, token);

        return Ok();
    }
}
