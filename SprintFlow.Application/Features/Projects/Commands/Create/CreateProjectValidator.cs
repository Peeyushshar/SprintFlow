using FluentValidation;

namespace SprintFlow.Application.Features.Projects.Commands.Create
{
    public class CreateProjectValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);

            RuleFor(x => x.Key)
                .NotEmpty()
                .MaximumLength(20)
                .Matches("^[A-Z0-9]+$")
                .WithMessage("Project key must contain only uppercase letters and numbers.");

            RuleFor(x => x.Description).MaximumLength(1000);
        }
    }
}
