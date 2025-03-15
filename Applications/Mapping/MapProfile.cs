using Application.Handlers.ProjectsHandler.Command;
using AutoMapper;
using Domain.Dtos.Project;
using Domain.Dtos.Services;
using Domain.Entities;

namespace Domain.Mapping;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<AddProjectCommand, AddProjectDto>().ReverseMap();
        CreateMap<PutProjectCommand, UpdateProjectDto>().ReverseMap();

        CreateMap<AddKubernetesServiceCommand, KubernetesServiceDto>().ReverseMap();
        CreateMap<AddKubernetesServiceCommand, AddKubernetesServiceDto>().ReverseMap();
        CreateMap<PutKubernetesServiceCommand, UpdateKubernetesServiceDto>().ReverseMap();

        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<Project, AddProjectDto>().ReverseMap();
        CreateMap<Project, UpdateProjectDto>().ReverseMap();

        CreateMap<Service, KubernetesServiceDto>().ReverseMap();
        CreateMap<Service, AddKubernetesServiceDto>().ReverseMap();
        CreateMap<Service, UpdateKubernetesServiceDto>().ReverseMap();

        CreateMap<KubernetesServiceDto, DeleteServiceDto>().ReverseMap();
        CreateMap<KubernetesServiceDto, AddKubernetesServiceDto>().ReverseMap();
    }
}
