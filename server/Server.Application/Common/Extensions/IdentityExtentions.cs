using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Server.Domain.Common.Constants;
using System.Security.Claims;

namespace Server.Application.Common.Extensions;

public static class IdentityExtentions
{
    public static List<Error> GetIdentityResultErrorDescriptions(this IdentityResult identityResult)
        => identityResult
            .Errors
            .Select(error => Error.Validation(code: error.Code, description: error.Description))
            .ToList();
    public static string GetSpecificClaim(this ClaimsIdentity claimsIdentity, string claimType)
    {
        var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == claimType);

        return (claim != null) ? claim.Value : string.Empty;
    }

    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var userId = ((ClaimsIdentity)claimsPrincipal.Identity!).GetSpecificClaim(UserClaims.Id);

        return Guid.Parse(userId);
    }

    public static Guid GetFacultyId(this ClaimsPrincipal claimsPrincipal)
    {
        var facultyId = ((ClaimsIdentity)claimsPrincipal.Identity!).GetSpecificClaim(UserClaims.FacultyId);
        return Guid.Parse(facultyId);
    }
}