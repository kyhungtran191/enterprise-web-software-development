using ErrorOr;

namespace Server.Domain.Common.Errors
{
    public static partial class  Errors
    {
        public static class AcademicYear
        {
            public static Error AlreadyExist => Error.Validation(code: "AcademicYear.AlreadyExist",
                description: "Academic Year already exists");

            public static Error NotFound =>
                Error.NotFound(code: "AcademicYear.NotFound", description: "Academic year not found");

            public static Error Deleted => Error.Validation(code: "AcademicYear.Deleted",
                description: "Academic Year has been deleted");

            public static Error ContributionExist => Error.Conflict(code: "AcademicYear.ContributionExist",
                description: "Can not delete. Still have contributions in this academic year ");

            public static Error HasActiveYearAlready => Error.Conflict(code: "AcademicYear.HasActiveYearAlready",
                description: "Has active academic year already");

            public static Error AlreadyInactive => Error.Validation(code: "AcademicYear.AlreadyInactive",
                description: "This academic year is not activated");
        }
        
    }
}
