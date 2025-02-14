using AutoMapper;
using Core.Dtos.Project;
using Core.Dtos.Services;
using Core.Entities;

namespace Domain.Mapping;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<Project, AddProjectRequest>().ReverseMap();
        CreateMap<Project, UpdateProjectRequest>().ReverseMap();

        CreateMap<Service, ServiceDto>().ReverseMap();
        CreateMap<Service, AddServiceRequest>().ReverseMap();
        CreateMap<Service, UpdateServiceRequest>().ReverseMap();

        CreateMap<ServiceDto, DeleteServiceDto>().ReverseMap();
        CreateMap<ServiceDto, AddServiceRequest>().ReverseMap();
    }
}
