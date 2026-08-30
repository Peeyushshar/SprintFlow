using SprintFlow.Application.Common.Interfaces.CQRS;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.Projects.Commands.Create
{
    public record CreateProjectCommand(string Name, string Key, string? Description)
        : ICommand<Result<CreateProjectResponse>>;
}
