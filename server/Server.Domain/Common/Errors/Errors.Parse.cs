using ErrorOr;

namespace Server.Domain.Common.Errors;

public static partial class Errors
{
    public static class Parse
    {
        public static Error CannotParse => Error.Validation(
            code: "Parse.CannotParse",
            description: "Can not Parse Value."
        );
    }
}