using Microsoft.AspNetCore.Identity;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.Authentication;
using SprintFlow.Application.Common.Interfaces.CQRS;
using SprintFlow.Application.Common.Models;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Features.Users.Commands.CreateUser
{
    public sealed class CreateUserCommandHandler
        : ICommandHandler<CreateUserCommand, Result<CreateUserResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ICurrentUserService _currentUser;

        public CreateUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            ICurrentUserService currentUser
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _currentUser = currentUser;
        }

        public async Task<Result<CreateUserResponse>> Handle(
            CreateUserCommand command,
            CancellationToken cancellationToken
        )
        {
            // ---------------------------------------------
            // Get current tenant
            // ---------------------------------------------

            var tenantId = _currentUser.TenantId;

            if (tenantId is null)
            {
                return Result<CreateUserResponse>.Failure(CommonErrors.TenantRequired);
            }

            // ---------------------------------------------
            // Check existing user
            // ---------------------------------------------

            var email = command.Email.Trim().ToLowerInvariant();

            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser is not null)
            {
                return Result<CreateUserResponse>.Failure(AuthErrors.EmailAlreadyExists);
            }

            // ---------------------------------------------
            // Validate roles
            // ---------------------------------------------

            var roleNames = new List<string>();

            foreach (var roleId in command.RoleIds.Distinct())
            {
                var role = await _roleManager.FindByIdAsync(roleId.ToString());

                if (role is null)
                {
                    return Result<CreateUserResponse>.Failure(AuthErrors.RoleNotFound);
                }

                roleNames.Add(role.Name!);
            }

            if (roleNames.Count == 0)
            {
                return Result<CreateUserResponse>.Failure(AuthErrors.RoleRequired);
            }

            // ---------------------------------------------
            // Create user
            // ---------------------------------------------

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = command.FirstName.Trim(),
                LastName = command.LastName.Trim(),
                IsActive = true,
                TenantId = tenantId.Value,
            };

            var userResult = await _userManager.CreateAsync(user, command.Password);

            if (!userResult.Succeeded)
            {
                return Result<CreateUserResponse>.Failure(AuthErrors.UserCreationFailed);
            }

            //set phonenumber

            var phoneNumberResult = await _userManager.SetPhoneNumberAsync(
                user,
                command.PhoneNumber
            );

            if (!phoneNumberResult.Succeeded)
            {
                return Result<CreateUserResponse>.Failure(AuthErrors.PhoneNumberAssignmentFailed);
            }
            // ---------------------------------------------
            // Assign roles
            // ---------------------------------------------

            var roleResult = await _userManager.AddToRolesAsync(user, roleNames);

            if (!roleResult.Succeeded)
            {
                return Result<CreateUserResponse>.Failure(AuthErrors.RoleAssignmentFailed);
            }

            // ---------------------------------------------
            // Response
            // ---------------------------------------------

            var response = new CreateUserResponse
            {
                TenantId = tenantId.Value,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                RoleIds = command.RoleIds.ToList(),
            };

            return Result<CreateUserResponse>.Success(response);
        }
    }
}
