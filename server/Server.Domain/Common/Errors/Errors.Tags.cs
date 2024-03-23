using ErrorOr;

namespace Server.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Tags
        {
            public static Error AlreadyExist => Error.Validation(
                code: "Tag.AlreadyExist", 
                description: "Tag already exist"
            );

            public static Error CannotFound => Error.NotFound(
                code: "Tag.NotFound",
                description: "Tag cannot found");

            public static Error Deleted => Error.Validation(
                code: "Tag.Deleted", description: "Tag has been deleted");
        }
    }
}
