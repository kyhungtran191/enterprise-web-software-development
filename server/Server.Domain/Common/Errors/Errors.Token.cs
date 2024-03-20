using ErrorOr;

namespace Server.Domain.Common.Errors;

public static partial class Errors
{
    public static class Token
    {
        public static Error Invalid => Error.Validation(
            code: "Token.Invalid",
            description: "Invalid Token."
        );

        public static Error Expired => Error.Validation(
            code: "Token.Expired",
            description: "Token has been expired."
        );
    }
}