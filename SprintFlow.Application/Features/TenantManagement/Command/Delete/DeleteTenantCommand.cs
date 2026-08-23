using SprintFlow.Application.Common.Interfaces.CQRS;

namespace SprintFlow.Application.Features.TenantManagement.Command.Delete
{
    public record DeleteTenantCommand(Guid TenantId) : ICommand;
}
