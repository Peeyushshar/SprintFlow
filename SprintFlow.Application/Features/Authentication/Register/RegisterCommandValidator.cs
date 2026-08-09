using FluentValidation;

namespace SprintFlow.Application.Features.Authentication.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(100);

            RuleFor(x => x.CompanySlug).NotEmpty().Matches("^[a-z0-9-]+$");

            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);

            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);

            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]")
                .Matches("[a-z]")
                .Matches("[0-9]");
        }
    }
}
