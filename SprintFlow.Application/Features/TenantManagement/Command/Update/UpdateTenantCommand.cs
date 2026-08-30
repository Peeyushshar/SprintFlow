using SprintFlow.Application.Common.Interfaces.CQRS;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.TenantManagement.Command.Update
{
    public record UpdateTenantCommand(Guid TenantId, string Name, string SubscriptionPlan)
        : ICommand<Result<UpdateTenantResponse>>;
}
