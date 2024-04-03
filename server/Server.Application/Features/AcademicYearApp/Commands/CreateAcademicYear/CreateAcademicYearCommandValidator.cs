using FluentValidation;

namespace Server.Application.Features.AcademicYearApp.Commands.CreateAcademicYear
{
    public class CreateAcademicYearCommandValidator : AbstractValidator<CreateAcademicYearCommand>
    {
        public CreateAcademicYearCommandValidator()
        {
            RuleFor(request => request.AcademicYearName)
                .NotEmpty()
                .WithMessage("Academic year name is required.")
                .Matches(@"^\d{4}-\d{4}$")
                .WithMessage("Academic year name must be in the format 'XXXX-YYYY'.")
                .Must(BeConsecutiveYears)
                .WithMessage("The years must be consecutive.");
            RuleFor(request => request.StartClosureDate)
                .Must((request, date) => IsWithinAcademicYear(date, request.AcademicYearName))
                .WithMessage("StartClosureDate must be within the academic year.");

            RuleFor(request => request.EndClosureDate)
                .GreaterThan(request => request.StartClosureDate)
                .WithMessage("EndClosureDate must be after StartClosureDate")
                .Must((request, date) => IsWithinOrAtEndOfAcademicYear(date, request.AcademicYearName))
                .WithMessage("EndClosureDate must be within the academic year or exactly at its end.");

            RuleFor(request => request.FinalClosureDate)
                .GreaterThan(request => request.EndClosureDate)
                .WithMessage("FinalClosureDate must be after EndClosureDate")
                .Must((request, date) => IsWithinOrAtEndOfAcademicYear(date, request.AcademicYearName))
                .WithMessage("FinalClosureDate must be within the academic year or exactly at its end.");
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
        private bool IsWithinAcademicYear(DateTime date, string academicYear)
        {
            var years = academicYear.Split('-');
            if (years.Length != 2 || !int.TryParse(years[0], out int startYear) || !int.TryParse(years[1], out int endYear))
            {
                return false;
            }

            var year = date.Year;
            return year >= startYear && year < endYear;
        }

        private bool IsWithinOrAtEndOfAcademicYear(DateTime date, string academicYear)
        {
            var years = academicYear.Split('-');
            if (years.Length != 2 || !int.TryParse(years[0], out int startYear) || !int.TryParse(years[1], out int endYear))
            {
                return false;
            }

            var academicYearEndDate = new DateTime(startYear, 12, 31);
            return date <= academicYearEndDate;
        }
    }
}
