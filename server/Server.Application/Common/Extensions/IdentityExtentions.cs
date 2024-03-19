using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace Server.Application.Common.Extensions;

public static class IdentityExtentions
{
    public static List<Error> GetIdentityResultErrorDescriptions(this IdentityResult identityResult)
        => identityResult
            .Errors
            .Select(error => Error.Validation(code: error.Code, description: error.Description))
            .ToList();
}