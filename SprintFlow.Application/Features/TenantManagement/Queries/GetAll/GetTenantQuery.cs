using MediatR;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.TenantManagement.Queries.GetAll
{
    public record class GetTenantQuery : IRequest<Result<IReadOnlyList<GetTenantResponse>>>;
}
