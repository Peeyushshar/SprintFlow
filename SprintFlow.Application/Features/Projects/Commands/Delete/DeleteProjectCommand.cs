using MediatR;
using SprintFlow.Application.Common.Interfaces.CQRS;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.Projects.Commands.Delete
{
    public record DeleteProjectCommand(Guid ProjectId) : ICommand;
}
