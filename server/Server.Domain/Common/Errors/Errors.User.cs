
using ErrorOr;

namespace Server.Domain.Common.Errors;

public static partial class Errors
{
    public static class User 
    {
        public static Error DuplicateEmail => Error.Validation(
            code: "User.DuplicateEmail",
            description: "User already exists."
        );

        public static Error CannotFound => Error.NotFound(
            code: "User.CannotFound",
            description: "User can not found."
        );

        public static Error CannotCreateNewUser => Error.Failure(
            code: "User.CannotCreateNewUser",
            description: "New user can not create."
        );

        public static Error InactiveOrEnableLocked => Error.Validation(
            code: "User.InactiveOrEnableLocked",
            description: "User is inactive or locked, please contact with Admin."
        );

        public static Error FailResetPassword => Error.Unexpected(code: "User.FailResetPassword",
            description: "Error occurs while reset password");

        public static Error NoAvatarFound => Error.Validation(code: "User.NoAvatarFound", description: "No Avatar Found");
    }
}