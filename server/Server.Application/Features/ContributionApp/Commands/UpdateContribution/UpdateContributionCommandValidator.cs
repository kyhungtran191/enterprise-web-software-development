using FluentValidation;

namespace Server.Application.Features.ContributionApp.Commands.UpdateContribution
{
    public class UpdateContributionCommandValidator : AbstractValidator<UpdateContributionCommand>
    {
        public UpdateContributionCommandValidator() { 
            RuleFor(x=>x.Title).NotNull().NotEmpty().MaximumLength(256).WithMessage("Title must be not empty and its length must be less than 256 ");
           
        }
    }
}
