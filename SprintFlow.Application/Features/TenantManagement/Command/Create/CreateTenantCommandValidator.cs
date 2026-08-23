using FluentValidation;

namespace SprintFlow.Application.Features.TenantManagement.Command.Create
{
    public sealed class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
    {
        public CreateTenantCommandValidator()
        {
            //-------------------------------------------------
            // Tenant
            //-------------------------------------------------

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Tenant name is required.")
                .MaximumLength(150)
                .WithMessage("Tenant name cannot exceed 150 characters.");

            RuleFor(x => x.Slug)
                .NotEmpty()
                .WithMessage("Tenant slug is required.")
                .MaximumLength(100)
                .WithMessage("Tenant slug cannot exceed 100 characters.")
                .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
                .WithMessage(
                    "Tenant slug can contain only lowercase letters, numbers and hyphens."
                );

            RuleFor(x => x.SubscriptionPlan)
                .NotEmpty()
                .WithMessage("Subscription plan is required.");

            //-------------------------------------------------
            // Owner
            //-------------------------------------------------

            RuleFor(x => x.OwnerFirstName)
                .NotEmpty()
                .WithMessage("Owner first name is required.")
                .MaximumLength(100)
                .WithMessage("Owner first name cannot exceed 100 characters.");

            RuleFor(x => x.OwnerLastName)
                .NotEmpty()
                .WithMessage("Owner last name is required.")
                .MaximumLength(100)
                .WithMessage("Owner last name cannot exceed 100 characters.");

            RuleFor(x => x.OwnerEmail)
                .NotEmpty()
                .WithMessage("Owner email is required.")
                .EmailAddress()
                .WithMessage("Owner email is not valid.")
                .MaximumLength(256)
                .WithMessage("Owner email cannot exceed 256 characters.");

            RuleFor(x => x.OwnerPassword)
                .NotEmpty()
                .WithMessage("Owner password is required.")
                .MinimumLength(8)
                .WithMessage("Owner password must be at least 8 characters.")
                .MaximumLength(100)
                .WithMessage("Owner password cannot exceed 100 characters.");
        }
    }
}
