using ErrorOr;

namespace Server.Domain.Common.Errors;

public static partial class Errors
{
    public static class RefreshToken
    {
        public static Error NotMatch => Error.Validation(
            code: "RefreshToken.NotMatch",
            description: "Refresh token is not match with user's refresh token ."
        );
  
    }
}