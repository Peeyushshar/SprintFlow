using global::SprintFlow.Application.Common.Errors;
using global::SprintFlow.Application.Common.Interfaces.Authentication;
using global::SprintFlow.Application.Common.Interfaces.CQRS;
using global::SprintFlow.Application.Common.Interfaces.Persistence;
using global::SprintFlow.Application.Common.Models;
using global::SprintFlow.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace SprintFlow.Application.Features.Users.Commands.UpdateUser
{
    public sealed class UpdateUserCommandHandler
        : ICommandHandler<UpdateUserCommand, Result<UpdateUserResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUser;

        public UpdateUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IUserRepository userRepository,
            ICurrentUserService currentUser
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<Result<UpdateUserResponse>> Handle(
            UpdateUserCommand command,
            CancellationToken cancellationToken
        )
        {
            // -------------------------------------------------
            // Get current tenant
            // -------------------------------------------------

            var tenantId = _currentUser.TenantId;

            if (tenantId is null)
            {
                return Result<UpdateUserResponse>.Failure(CommonErrors.TenantRequired);
            }

            // -------------------------------------------------
            // Get user
            // -------------------------------------------------

            var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);

            if (user is null)
            {
                return Result<UpdateUserResponse>.Failure(CommonErrors.EntityNotFound);
            }

            // -------------------------------------------------
            // Normalize email
            // -------------------------------------------------

            var email = command.Email.Trim().ToLowerInvariant();

            // -------------------------------------------------
            // Check email
            // -------------------------------------------------

            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser is not null && existingUser.Id != user.Id)
            {
                return Result<UpdateUserResponse>.Failure(AuthErrors.EmailAlreadyExists);
            }

            // -------------------------------------------------
            // Validate roles
            // -------------------------------------------------

            var roleNames = new List<string>();

            foreach (var roleId in command.RoleIds.Distinct())
            {
                var role = await _roleManager.FindByIdAsync(roleId.ToString());

                if (role is null)
                {
                    return Result<UpdateUserResponse>.Failure(AuthErrors.RoleNotFound);
                }

                roleNames.Add(role.Name!);
            }

            if (roleNames.Count == 0)
            {
                return Result<UpdateUserResponse>.Failure(AuthErrors.RoleRequired);
            }

            // -------------------------------------------------
            // Update basic information
            // -------------------------------------------------

            user.FirstName = command.FirstName.Trim();
            user.LastName = command.LastName.Trim();

            var emailResult = await _userManager.SetEmailAsync(user, email);

            if (!emailResult.Succeeded)
            {
                return Result<UpdateUserResponse>.Failure(AuthErrors.UserUpdateFailed);
            }

            user.UserName = email;

            // -------------------------------------------------
            // Update phone number
            // -------------------------------------------------

            var phoneResult = await _userManager.SetPhoneNumberAsync(
                user,
                command.PhoneNumber.Trim()
            );

            if (!phoneResult.Succeeded)
            {
                return Result<UpdateUserResponse>.Failure(AuthErrors.PhoneNumberAssignmentFailed);
            }

            // -------------------------------------------------
            // Update roles
            // -------------------------------------------------

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeRolesResult.Succeeded)
            {
                return Result<UpdateUserResponse>.Failure(AuthErrors.RoleAssignmentFailed);
            }

            var addRolesResult = await _userManager.AddToRolesAsync(user, roleNames);

            if (!addRolesResult.Succeeded)
            {
                return Result<UpdateUserResponse>.Failure(AuthErrors.RoleAssignmentFailed);
            }

            // -------------------------------------------------
            // Save
            // -------------------------------------------------

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return Result<UpdateUserResponse>.Failure(AuthErrors.UserUpdateFailed);
            }

            // -------------------------------------------------
            // Response
            // -------------------------------------------------

            var roleIds = command.RoleIds.Distinct().ToList();

            var response = new UpdateUserResponse
            {
                UserId = user.Id,
                TenantId = user.TenantId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                RoleIds = roleIds,
                IsActive = user.IsActive,
            };

            return Result<UpdateUserResponse>.Success(response);
        }
    }
}
