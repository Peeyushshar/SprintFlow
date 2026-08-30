using Microsoft.AspNetCore.Identity;
using SprintFlow.Application.Common.Errors;
using SprintFlow.Application.Common.Interfaces.CQRS;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.Models;
using SprintFlow.Domain.Constants;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Application.Features.TenantManagement.Command.Create
{
    public sealed class CreateTenantCommandHandler
        : ICommandHandler<CreateTenantCommand, Result<CreateTenantResponse>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTenantCommandHandler(
            ITenantRepository tenantRepository,
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork
        )
        {
            _tenantRepository = tenantRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateTenantResponse>> Handle(
            CreateTenantCommand command,
            CancellationToken cancellationToken
        )
        {
            var tenantName = command.Name.Trim();

            var slug = command.Slug.Trim().ToLowerInvariant();

            var ownerEmail = command.OwnerEmail.Trim().ToLowerInvariant();

            var tenantExists = await _tenantRepository.ExistsBySlugAsync(slug, cancellationToken);

            if (tenantExists)
            {
                return Result<CreateTenantResponse>.Failure(TenantErrors.SlugAlreadyExists);
            }

            var existingUser = await _userManager.FindByEmailAsync(ownerEmail);

            if (existingUser is not null)
            {
                return Result<CreateTenantResponse>.Failure(TenantErrors.OwnerEmailAlreadyExists);
            }

            //-------------------------------------------------
            // Begin transaction
            //-------------------------------------------------

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                //-------------------------------------------------
                // Create Tenant
                //-------------------------------------------------

                var tenant = new Tenant
                {
                    Name = tenantName,
                    Slug = slug,
                    SubscriptionPlan = command.SubscriptionPlan,
                    IsActive = true,
                };

                await _tenantRepository.CreateAsync(tenant, cancellationToken);

                //-------------------------------------------------
                // Create Tenant Owner
                //-------------------------------------------------

                var owner = new ApplicationUser
                {
                    UserName = ownerEmail,
                    Email = ownerEmail,

                    FirstName = command.OwnerFirstName.Trim(),
                    LastName = command.OwnerLastName.Trim(),

                    TenantId = tenant.Id,

                    IsActive = true,
                };

                var userResult = await _userManager.CreateAsync(owner, command.OwnerPassword);

                //-------------------------------------------------
                // Identity validation failed
                //-------------------------------------------------

                if (!userResult.Succeeded)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);

                    var errors = string.Join(", ", userResult.Errors.Select(x => x.Description));

                    return Result<CreateTenantResponse>.Failure(AuthErrors.UserCreationFailed);
                }

                //-------------------------------------------------
                // Assign Owner role
                //-------------------------------------------------

                var roleResult = await _userManager.AddToRoleAsync(owner, Roles.Owner);

                if (!roleResult.Succeeded)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);

                    var errors = string.Join(", ", roleResult.Errors.Select(x => x.Description));

                    return Result<CreateTenantResponse>.Failure(AuthErrors.OwnerRoleMissing);
                }

                //-------------------------------------------------
                // Save everything
                //-------------------------------------------------

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                //-------------------------------------------------
                // Commit
                //-------------------------------------------------

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                //-------------------------------------------------
                // Response
                //-------------------------------------------------

                var response = new CreateTenantResponse
                {
                    TenantId = tenant.Id,
                    TenantName = tenant.Name,
                    Slug = tenant.Slug,
                    SubscriptionPlan = tenant.SubscriptionPlan,
                    OwnerId = owner.Id,
                    OwnerEmail = owner.Email!,
                };

                return Result<CreateTenantResponse>.Success(response);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);

                throw;
            }
        }
    }
}
