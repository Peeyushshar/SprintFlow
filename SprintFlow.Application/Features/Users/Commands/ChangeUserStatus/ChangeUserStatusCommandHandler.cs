using Microsoft.AspNetCore.Identity;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.Authentication;
using SprintFlow.Application.Common.Interfaces.CQRS;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Features.Users.Commands.ChangeUserStatus
{
    public sealed class ChangeUserStatusCommandHandler
        : ICommandHandler<ChangeUserStatusCommand, Result<ChangeUserStatusResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUser;

        public ChangeUserStatusCommandHandler(
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            ICurrentUserService currentUser
        )
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<Result<ChangeUserStatusResponse>> Handle(
            ChangeUserStatusCommand command,
            CancellationToken cancellationToken
        )
        {
            var tenantId = _currentUser.TenantId;

            if (tenantId is null)
            {
                return Result<ChangeUserStatusResponse>.Failure(CommonErrors.TenantRequired);
            }

            var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);

            if (user is null)
            {
                return Result<ChangeUserStatusResponse>.Failure(CommonErrors.EntityNotFound);
            }

            user.IsActive = command.IsActive;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Result<ChangeUserStatusResponse>.Failure(AuthErrors.UserUpdateFailed);
            }

            return Result<ChangeUserStatusResponse>.Success(
                new ChangeUserStatusResponse { UserId = user.Id, IsActive = user.IsActive }
            );
        }
    }
}
