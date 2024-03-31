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

            public static Error AlreadyApproved => Error.Conflict(code: "Contribution.AlreadyApproved",
                description: "This contribution has already been approved");
            public static Error AlreadyRejected => Error.Conflict(code: "Contribution.AlreadyRejected",
                description: "This contribution has already been rejected");

            public static Error NotRejected => Error.Conflict(code: "Contribution.NotRejected",
                description: "This contribution is not rejected");

            public static Error NoFilesFound => Error.NotFound(code: "Contribution.NoFilesFound",
                description: "Contribution do not have any files");

            public static Error NotFoundPublic => Error.NotFound(code: "ContributionPublic.NotFound",
                description: "This contribution not found or not public");
        }
    }
}
