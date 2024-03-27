using FluentValidation;
using Server.Application.Features.ContributionApp.Queries.GetContributionByTitle;

namespace Server.Application.Features.ContributionApp.Queries.GetContributionBySlug
{
    public class GetContributionBySlugQueryValidator : AbstractValidator<GetContributionBySlugQuery>
    {
        public GetContributionBySlugQueryValidator()
        {
            RuleFor(x=>x.Slug)
                .NotNull()
                .NotEmpty().WithMessage("Slug must be not empty")
                .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Must be a valid slug format.");
        }
    }
}
