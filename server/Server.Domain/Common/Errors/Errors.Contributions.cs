using ErrorOr;

namespace Server.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Contribution
        {
            public static Error NotFound => Error.NotFound(code:"Contribution.NotFound",description:"Contribution not found");

            public static Error AlreadyExist => Error.Validation(code: "Contribution.AlreadyExist",
                description: "Contribution Already Exist");

            public static Error AcademicYearNotFound => Error.NotFound(code: "Contribution.AcademicYearNotFound",
                description: "Academic year not found to create contribution");

            public static Error Deleted =>
                Error.Conflict(code: "Contribution.Deleted", description: "Contribution has been deleted");

            public static Error NotConfirmed =>
                Error.Validation("Contribution.NotConfirmed", "Please accept term and condition before submitting");

            public static Error SlugExist => Error.Validation(code: "Contribution.SlugExist",description:"Please enter another title");
        }
    }
}
