using SprintFlow.Application.Common.Interfaces.CQRS;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.TenantManagement.Command.Create
{
    public record CreateTenantCommand(
        string Name,
        string Slug,
        string SubscriptionPlan,
        string OwnerFirstName,
        string OwnerLastName,
        string OwnerEmail,
        string OwnerPassword
    ) : ICommand<Result<CreateTenantResponse>>;
}
