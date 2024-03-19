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
    }
}