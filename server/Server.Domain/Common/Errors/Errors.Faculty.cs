using ErrorOr;

namespace Server.Domain.Common.Errors;

public static partial class Errors
{
    public static class Faculty
    {
        public static Error CannotFound => Error.NotFound(
            code: "Faculty.CannotFound",
            description: "Faculty can not found."
        );

        public static Error AlreadyExisting => Error.Validation(
            code: "Faculty.AlreadyExisting",
            description: "Faculty already existing."
        );

        public static Error HasInUser => Error.Validation(
            code: "Faculty.HasInUser",
            description: "Faculty is found in Users so we can not update it."
        );
    }
}