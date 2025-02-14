using AutoMapper;
using Core.Dtos.Project;
using Core.Dtos.Services;
using Core.Entities;
using Core.Models;
using Core.Services;
using Domain.Factory.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Controllers;

[ApiController]
[Route("api/services")]
public class ServiceController : ControllerBase
{
    private readonly ServiceFactory _serviceFactory;
    private readonly IServiceService _serviceService;
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public ServiceController(ServiceFactory serviceFactory, IServiceService serviceService, IProjectService projectService, IMapper mapper)
    {
        _serviceFactory = serviceFactory;
        _serviceService = serviceService;
        _projectService = projectService;
        _mapper = mapper;
    }

    [HttpPost()]
    public async Task<IActionResult> CreateService([FromBody] AddServiceRequest request, CancellationToken token)
    {
        var handler = _serviceFactory.GetServiceHandler(request.ServiceType);

        var project = await _projectService.Get(request.ProjectId);

        var serviceDto = _mapper.Map<ServiceDto>(request);

        var kube = await handler.AddServiceToUserPodAsync(
            serviceDto, project
        );

        request.Host = kube.Host;
        request.User = kube.User;
        request.Password = kube.Password;

        var service = await _serviceService.Add(request, token);

        return Ok(service);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteService(string id, CancellationToken token)
    {
        var service = await _serviceService.Delete(id, token);

        var handler = _serviceFactory.GetServiceHandler(service.ServiceType);

        await handler.DeleteServiceAsync(service);

        return Ok();
    }

    /// <summary>
    /// get a Service by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [SwaggerResponse(200, "Service details retrieved successfully.", typeof(ServiceDto))]
    [SwaggerResponse(404, "Service not found.")]
    public async Task<IActionResult> Get(string id)
    {
        var serviceDto = await _serviceService.Get(id);

        var handler = _serviceFactory.GetServiceHandler(serviceDto.ServiceType);

        var status = await handler.GetServiceStatusAsync(serviceDto);
        var metrics = await handler.GetServiceMetricsAsync(serviceDto);

        serviceDto.ContainerStatusType = status;
        serviceDto.ContainerMetrics = metrics;

        return Ok(serviceDto);
    }

    /// <summary>
    /// get a list of Services
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(200, "Services retrieved successfully.", typeof(Pagination<ServiceDto>))]
    public async Task<IActionResult> Get(int pageIndex = 0, int pageSize = 10)
    {
        var servicesDto = await _serviceService.Get(pageIndex, pageSize);

        foreach (var serviceDto in servicesDto.Items)
        {
            var handler = _serviceFactory.GetServiceHandler(serviceDto.ServiceType);

            var status = await handler.GetServiceStatusAsync(serviceDto);
            var metrics = await handler.GetServiceMetricsAsync(serviceDto);

            serviceDto.ContainerStatusType = status;
            serviceDto.ContainerMetrics = metrics;
        }
       
        return Ok(servicesDto);
    }

    /// <summary>
    /// update a Service
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPut]
    [SwaggerResponse(200, "Service updated successfully.", typeof(ServiceDto))]
    [SwaggerResponse(400, "Invalid request.")]
    [SwaggerResponse(404, "Service not found.")]
    public async Task<IActionResult> Update(UpdateServiceRequest request, CancellationToken token)
        => Ok(await _serviceService.Update(request, token));
}
