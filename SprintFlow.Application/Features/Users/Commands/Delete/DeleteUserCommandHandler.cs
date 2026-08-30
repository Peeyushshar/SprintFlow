using MediatR;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;

namespace SprintFlow.Application.Features.Users.Commands.Delete
{
    public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(
            DeleteUserCommand command,
            CancellationToken cancellationToken
        )
        {
            var result = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);

            if (result is null)
            {
                return Result.Failure(CommonErrors.EntityNotFound);
            }

            await _userRepository.DeleteAsync(result, cancellationToken);

            return Result.Success();
        }
    }
}
