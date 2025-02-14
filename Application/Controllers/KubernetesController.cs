using Core.Dtos.Kubernetes;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/kubernetes")]
public class KubernetesController : ControllerBase
{
    private readonly KubernetesManager _kubernetesManager;

    public KubernetesController(KubernetesManager kubernetesManager)
    {
        _kubernetesManager = kubernetesManager;
    }

    [HttpPost("namespace")]
    public async Task<IActionResult> CreateNamespace([FromBody] CreateNamespaceDto createNamespace)
    {
        var response = await _kubernetesManager.CreateNamespaceAsync(createNamespace);

        return Ok(response);
    }
}
