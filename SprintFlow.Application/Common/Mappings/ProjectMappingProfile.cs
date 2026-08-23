using AutoMapper;
using SprintFlow.Application.Features.Projects.Commands.Create;
using SprintFlow.Application.Features.Projects.Update;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Common.Mappings
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, CreateProjectResponse>();

            // Update Command -> Existing Project
            CreateMap<UpdateProjectCommand, Project>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TenantId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());

            // Update response
            CreateMap<Project, UpdateProjectResponse>();
        }
    }
}
