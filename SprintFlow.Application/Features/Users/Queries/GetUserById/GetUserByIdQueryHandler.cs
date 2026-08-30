using MediatR;
using Microsoft.AspNetCore.Identity;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.Authentication;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Features.Users.Queries.GetUserById
{
    public sealed class GetUserByIdQueryHandler
        : IRequestHandler<GetUserByIdQuery, Result<GetUsersResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUser;

        public GetUserByIdQueryHandler(
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUser
        )
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _currentUser = currentUser;
        }

        public async Task<Result<GetUsersResponse>> Handle(
            GetUserByIdQuery query,
            CancellationToken cancellationToken
        )
        {
            if (_currentUser.TenantId is null)
            {
                return Result<GetUsersResponse>.Failure(CommonErrors.TenantRequired);
            }

            var user = await _userRepository.GetByIdAsync(query.UserId, cancellationToken);

            if (user is null)
            {
                return Result<GetUsersResponse>.Failure(CommonErrors.EntityNotFound);
            }

            var roles = await _userManager.GetRolesAsync(user);

            var response = new GetUsersResponse
            {
                Id = user.Id,
                TenantId = user.TenantId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                Roles = roles.ToList(),
            };

            return Result<GetUsersResponse>.Success(response);
        }
    }
}
