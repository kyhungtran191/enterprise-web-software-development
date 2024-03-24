using FluentValidation;

namespace Server.Application.Features.AcademicYearApp.Commands.CreateAcademicYear
{
    public class CreateAcademicYearCommandValidator : AbstractValidator<CreateAcademicYearCommand>
    {
        public CreateAcademicYearCommandValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .WithMessage("Academic year name is required.");
        }
    }
}
