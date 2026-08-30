using AutoMapper;
using SprintFlow.Application.Features.TenantManagement.Command.Update;
using SprintFlow.Application.Features.TenantManagement.Queries;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Common.Mappings
{
    public class TenantMappingProfiles : Profile
    {
        public TenantMappingProfiles()
        {
            CreateMap<Tenant, GetTenantResponse>();
            CreateMap<Tenant, UpdateTenantResponse>();
        }
    }
}
