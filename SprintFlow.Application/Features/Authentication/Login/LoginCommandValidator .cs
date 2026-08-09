using FluentValidation;

namespace SprintFlow.Application.Features.Authentication.Login
{
    internal class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);

            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        }
    }
}
