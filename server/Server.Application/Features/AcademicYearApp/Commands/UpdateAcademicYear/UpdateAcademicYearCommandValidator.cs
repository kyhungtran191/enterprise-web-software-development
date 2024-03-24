using FluentValidation;

namespace Server.Application.Features.AcademicYearApp.Commands.UpdateAcademicYear
{
    public class UpdateAcademicYearCommandValidator : AbstractValidator<UpdateAcademicYearCommand>
    {
        public UpdateAcademicYearCommandValidator()
        {
            RuleFor(x => x.AcademicYearName).NotEmpty().WithMessage("Academic year name is required");
        }
    }
}
