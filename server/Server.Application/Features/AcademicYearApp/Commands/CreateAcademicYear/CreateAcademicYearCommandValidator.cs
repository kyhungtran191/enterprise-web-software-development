using FluentValidation;

namespace Server.Application.Features.AcademicYearApp.Commands.CreateAcademicYear
{
    public class CreateAcademicYearCommandValidator : AbstractValidator<CreateAcademicYearCommand>
    {
        public CreateAcademicYearCommandValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .WithMessage("Academic year name is required.")
                .Matches(@"^\d{4}-\d{4}$")
                .WithMessage("Academic year name must be in the format 'XXXX-YYYY'.")
                .Must(BeConsecutiveYears)
                .WithMessage("The years must be consecutive.");
        }
        private bool BeConsecutiveYears(string name)
        {
            var years = name.Split('-');
            if (years.Length != 2)
            {
                return false;
            }

            if (int.TryParse(years[0], out int startYear) && int.TryParse(years[1], out int endYear))
            {
                return endYear - startYear == 1;
            }

            return false;
        }
    }
}
