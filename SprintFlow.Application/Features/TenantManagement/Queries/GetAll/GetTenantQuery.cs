using MediatR;
using SprintFlow.Application.Common.Models;
using SprintFlow.Application.Features.Projects.Queries.GetProject;

namespace SprintFlow.Application.Features.TenantManagement.Queries.GetAll
{
    public record class GetTenantQuery : IRequest<Result<IReadOnlyList<GetTenantResponse>>>;
}
