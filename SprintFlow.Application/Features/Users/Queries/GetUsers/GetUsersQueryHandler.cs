using MediatR;
using Microsoft.AspNetCore.Identity;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.Authentication;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Features.Users.Queries.GetUsers
{
    public sealed class GetUsersQueryHandler
        : IRequestHandler<GetUsersQuery, Result<IReadOnlyList<GetUsersResponse>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUser;

        public GetUsersQueryHandler(
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUser
        )
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _currentUser = currentUser;
        }

        public async Task<Result<IReadOnlyList<GetUsersResponse>>> Handle(
            GetUsersQuery query,
            CancellationToken cancellationToken
        )
        {
            if (_currentUser.TenantId is null)
            {
                return Result<IReadOnlyList<GetUsersResponse>>.Failure(CommonErrors.TenantRequired);
            }

            var users = await _userRepository.GetAllAsync(cancellationToken);

            var response = new List<GetUsersResponse>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                response.Add(
                    new GetUsersResponse
                    {
                        Id = user.Id,
                        TenantId = user.TenantId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email!,
                        PhoneNumber = user.PhoneNumber,
                        IsActive = user.IsActive,
                        Roles = roles.ToList(),
                    }
                );
            }

            return Result<IReadOnlyList<GetUsersResponse>>.Success(response);
        }
    }
}
