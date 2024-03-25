using FluentValidation;

namespace Server.Application.Features.ContributionApp.Commands.CreateContribution
{
    public class CreateContributionCommandValidator : AbstractValidator<CreateContributionCommand>
    {
        public CreateContributionCommandValidator()
        {
            RuleFor(x=>x.Title).NotNull().NotEmpty().MaximumLength(256).WithMessage("Title must be not empty and its length must be less than 256 ");
            RuleFor(x => x.AcademicYearId).NotNull().NotEmpty()
                .WithMessage("Please select academic year before submit contribution ");
        }
    }
}
