using FluentValidation;

namespace Server.Application.Features.PublicContributionApp.Commands.RateContribution
{
    public class RateContributionValidator : AbstractValidator<RateContributionCommand>
    {
        public RateContributionValidator()
        {
            RuleFor(x => x.Rating)
                .InclusiveBetween(0.0, 5.0).WithMessage("Rating must be between 0.0 and 5.0");
        }
    }
}
