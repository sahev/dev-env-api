using AutoMapper;
using Core.Dtos.Project;
using Core.Entities;
using Core.Exceptions;
using Core.Models;
using Core.Repositories;
using Core.Services;
using Domain.Factory.Services;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services;

public class ProjectService(IUnitOfWork unitOfWork, IMapper mapper) : IProjectService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Pagination<ProjectDto>> Get(int pageIndex, int pageSize)
    {
        var projects = await _unitOfWork.ProjectRepository.ToPagination(
            pageIndex: pageIndex,
            pageSize: pageSize,
            orderBy: x => x.Name,
            ascending: true,
            include: x => x.Include(x => x.Services),
            selector: x => new ProjectDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                StorageSize = x.StorageSize,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                Services = x.Services,
            }
        );

        return projects;
    }

    public async Task<ProjectDto> Get(string id)
    {
        var project = await _unitOfWork.ProjectRepository.FirstOrDefaultAsync(x => x.Id == id);
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> Add(AddProjectRequest request, CancellationToken token)
    {
        var project = _mapper.Map<Project>(request);
        await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.ProjectRepository.AddAsync(project), token);
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> Update(UpdateProjectRequest request, CancellationToken token)
    {
        if (!await _unitOfWork.ProjectRepository.AnyAsync(x => x.Id == request.Id))
            throw new UserFriendlyException("project not found", "project not found");

        var project = _mapper.Map<Project>(request);
        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.ProjectRepository.Update(project), token);
        return _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> Delete(string id, CancellationToken token)
    {
        var existproject = await _unitOfWork.ProjectRepository.FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new UserFriendlyException("project not found", "project not found");

        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.ProjectRepository.Delete(existproject), token);
        return _mapper.Map<ProjectDto>(existproject);
    }
}
