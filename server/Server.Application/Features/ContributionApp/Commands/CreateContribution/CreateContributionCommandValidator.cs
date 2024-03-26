using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Server.Application.Features.ContributionApp.Commands.CreateContribution
{
    public class CreateContributionCommandValidator : AbstractValidator<CreateContributionCommand>
    {
        public CreateContributionCommandValidator()
        {
            RuleFor(x=>x.Title).NotNull().NotEmpty().MaximumLength(256).WithMessage("Title must be not empty and its length must be less than 256 ");
            RuleFor(x => x.Slug)
                .NotNull()
                .NotEmpty()
                .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
                .WithMessage("Slug must not be empty and must be a valid slug format.");

            RuleFor(x => x.AcademicYearId)
                .NotEmpty()
                .WithMessage("Please select an academic year before submitting the contribution.");

            RuleFor(x => x.FacultyId)
                .NotEmpty()
                .WithMessage("Faculty ID must not be empty.");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID must not be empty.");


        }
    }

}
