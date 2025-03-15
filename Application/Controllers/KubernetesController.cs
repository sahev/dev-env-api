using Application.Handlers.KubernetesHandler.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/kubernetes")]
public class KubernetesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("namespace")]
    public async Task<IActionResult> CreateNamespace([FromBody] AddNamespaceCommand request)
    {
        var response = await _mediator.Send(request);

        return Ok(response);
    }
}
