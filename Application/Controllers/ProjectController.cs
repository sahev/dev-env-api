using AutoMapper;
using Core.Dtos.Project;
using Core.Entities;
using Core.Exceptions;
using Core.Models;
using Core.Services;
using Domain.Factory.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.RegularExpressions;

namespace Application.Controllers;

public class ProjectController(IProjectService projectService, ServiceFactory serviceFactory, IServiceService serviceService, IMapper mapper) : BaseController
{
    private readonly IProjectService _projectService = projectService;
    private readonly ServiceFactory _serviceFactory = serviceFactory;
    private readonly IServiceService _serviceService = serviceService;
    private readonly IMapper _mapper = mapper; 

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
        var project = await _projectService.Get(id);

        foreach (var service in project.Services)
        {
            var handler = _serviceFactory.GetServiceHandler(service.ServiceType);

            var serviceDto = _mapper.Map<ServiceDto>(service);

            var status = await handler.GetServiceStatusAsync(serviceDto);
            var metrics = await handler.GetServiceMetricsAsync(serviceDto);

            project.ContainerMetrics.Usage.Cpu += metrics.Usage.Cpu;
            project.ContainerMetrics.Usage.Memory += metrics.Usage.Memory;
            project.ContainerMetrics.UpTime += metrics.UpTime;

            service.ContainerStatusType = status;
            service.ContainerMetrics = metrics;
        }

        return Ok(project);
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
        var projects = await _projectService.Get(pageIndex, pageSize);

        foreach (var project in projects.Items)
        {
            foreach (var service in project.Services)
            {
                var handler = _serviceFactory.GetServiceHandler(service.ServiceType);

                var serviceDto = _mapper.Map<ServiceDto>(service);

                var status = await handler.GetServiceStatusAsync(serviceDto);
                var metrics = await handler.GetServiceMetricsAsync(serviceDto);

                project.ContainerMetrics.Usage.Cpu += service.ContainerMetrics.Usage.Cpu;
                project.ContainerMetrics.Usage.Memory += service.ContainerMetrics.Usage.Memory;
                project.ContainerMetrics.UpTime += metrics.UpTime;

                service.ContainerStatusType = status;
                service.ContainerMetrics = metrics;
            }
        }

        return Ok(projects);
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
    public async Task<IActionResult> Add(AddProjectRequest request, CancellationToken token)
        => Ok(await _projectService.Add(request, token));

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
    public async Task<IActionResult> Update(UpdateProjectRequest request, CancellationToken token)
        => Ok(await _projectService.Update(request, token));

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
        var project = await _projectService.Get(id) ?? throw new UserFriendlyException("project not found", "project not found");

        foreach (Service service in project.Services)
        {
            await _serviceService.Delete(service.Id, token);

            var handler = _serviceFactory.GetServiceHandler(service.ServiceType);

            var serviceDto = _mapper.Map<ServiceDto>(service);

            await handler.DeleteServiceAsync(serviceDto);
        }

        await _projectService.Delete(id, token);

        return Ok();
    }
}
