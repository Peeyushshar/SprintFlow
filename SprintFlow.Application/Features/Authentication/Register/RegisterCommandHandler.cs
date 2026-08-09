using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.Authentication;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;
using SprintFlow.Application.Enums;
using SprintFlow.Domain.Constants;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Features.Authentication.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITenantRepository _tenantRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            ITenantRepository tenantRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IUnitOfWork unitOfWork,
            ILogger<RegisterCommandHandler> logger
        )
        {
            _userManager = userManager;
            _tenantRepository = tenantRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<RegisterResponse>> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken
        )
        {
            // Check Company Slug
            if (await _tenantRepository.ExistsBySlugAsync(request.CompanySlug))
            {
                return Result<RegisterResponse>.Failure(TenantErrors.SlugAlreadyExists);
            }

            // Check Email
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return Result<RegisterResponse>.Failure(AuthErrors.EmailAlreadyExists);
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                //-------------------------------------------------
                // Create Tenant
                //-------------------------------------------------

                var tenant = new Tenant
                {
                    Id = Guid.NewGuid(),
                    Name = request.CompanyName,
                    Slug = request.CompanySlug,
                };

                await _tenantRepository.AddAsync(tenant);

                //-------------------------------------------------
                // Create User
                //-------------------------------------------------

                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenant.Id,
                    UserName = request.Email,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    EmailConfirmed = false,
                };

                var createUserResult = await _userManager.CreateAsync(user, request.Password);

                if (!createUserResult.Succeeded)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);

                    var message = string.Join(
                        ", ",
                        createUserResult.Errors.Select(e => e.Description)
                    );

                    return Result<RegisterResponse>.Failure(
                        new Error(ErrorCodes.UserCreationFailed, message, ErrorType.Failure)
                    );
                }

                //-------------------------------------------------
                // Assign Owner Role
                //-------------------------------------------------

                await _userManager.AddToRoleAsync(user, Roles.Owner);

                //-------------------------------------------------
                // Save
                //-------------------------------------------------

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                //-------------------------------------------------
                // Response
                //-------------------------------------------------

                return Result<RegisterResponse>.Success(
                    new RegisterResponse
                    {
                        UserId = user.Id,
                        TenantId = tenant.Id,
                        Email = request.Email,
                        Role = Roles.Owner,
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed for {Email}", request.Email);

                await _unitOfWork.RollbackTransactionAsync(cancellationToken);

                return Result<RegisterResponse>.Failure(
                    new Error(
                        ErrorCodes.Unexpected,
                        "Unexpected server error.",
                        ErrorType.Forbidden
                    )
                );
            }
        }
    }
}
