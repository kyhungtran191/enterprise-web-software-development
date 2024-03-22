using ErrorOr;

namespace Server.Domain.Common.Errors;

public static partial class Errors
{
    public static class Roles
    {
        public static Error NameDuplicated =>
            Error.Validation(
                code: "Roles.NameDuplicated",
                description: "Role name is duplicated. Please create other role name."
            );

        public static Error DeleteFailed =>
            Error.Validation(
                code: "Roles.DeleteFailed",
                description: "The deletion of the role failed."
            );
        public static Error NotFound =>
            Error.Validation(
                code: "Roles.RoleNameDuplicated",
                description: "Role name is duplicated. Please create other role name."
            );
        public static Error EmptyId =>
            Error.Validation(
                code: "Roles.EmptyId",
                description: "Role Id is required."
            );
    }
}