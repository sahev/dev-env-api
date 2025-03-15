using Application.Handlers.ProjectsHandler.Command;
using Application.Handlers.ProjectsHandler.Query;
using Domain.Dtos.Project;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Controllers;

[ApiController]
public class KubernetesServiceController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost()]
    public async Task<IActionResult> CreateService([FromBody] AddKubernetesServiceCommand request, CancellationToken token)
    {
        return Ok(await _mediator.Send(request, token));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(string id, CancellationToken token)
    {
        await _mediator.Send((DeleteKubernetesServiceCommand)id, token);

        return Ok();
    }

    /// <summary>
    /// get a Service by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [SwaggerResponse(200, "Service details retrieved successfully.", typeof(KubernetesServiceDto))]
    [SwaggerResponse(404, "Service not found.")]
    public async Task<IActionResult> Get(string id)
    {
        return Ok(await _mediator.Send((GetKubernetesServiceQuery)id));
    }

    /// <summary>
    /// get a list of Services
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(200, "Services retrieved successfully.", typeof(Pagination<KubernetesServiceDto>))]
    public async Task<IActionResult> Get(int pageIndex = 0, int pageSize = 10)
    {
        return Ok(_mediator.Send(new GetAllKubernetesServiceQuery { PageIndex = pageIndex, PageSize = pageSize }));
    }

    /// <summary>
    /// update a Service
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPut]
    [SwaggerResponse(200, "Service updated successfully.", typeof(KubernetesServiceDto))]
    [SwaggerResponse(400, "Invalid request.")]
    [SwaggerResponse(404, "Service not found.")]
    public async Task<IActionResult> Update(PutKubernetesServiceCommand request, CancellationToken token)
        => Ok(await _mediator.Send(request, token));
}
