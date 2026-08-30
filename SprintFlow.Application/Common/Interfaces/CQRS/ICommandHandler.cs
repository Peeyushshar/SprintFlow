using MediatR;

namespace SprintFlow.Application.Common.Interfaces.CQRS
{
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse> { }
}
