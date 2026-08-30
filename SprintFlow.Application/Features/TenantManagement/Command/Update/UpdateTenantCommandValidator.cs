using FluentValidation;
using SprintFlow.Application.Features.TenantManagement.Command.Create;

namespace SprintFlow.Application.Features.TenantManagement.Command.Update
{
    public sealed class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
    {
        public UpdateTenantCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Tenant name is required.")
                .MaximumLength(150)
                .WithMessage("Tenant name cannot exceed 150 characters.");

            RuleFor(x => x.SubscriptionPlan)
                .NotEmpty()
                .WithMessage("Subscription plan is required.");
        }
    }
}
