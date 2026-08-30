using MediatR;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.TenantManagement.Queries.GetById
{
    public record GetTenantByIdQuery(Guid TenantId) : IRequest<Result<GetTenantResponse>>;
}
