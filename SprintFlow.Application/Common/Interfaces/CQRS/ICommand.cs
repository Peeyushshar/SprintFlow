using MediatR;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Common.Interfaces.CQRS
{
    public interface ICommand<out TResponse> : IRequest<TResponse> { }

    public interface ICommand : IRequest<Result> { }
}
