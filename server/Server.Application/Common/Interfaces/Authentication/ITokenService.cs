using System.Security.Claims;

namespace Server.Application.Common.Interfaces.Authentication;

public interface ITokenService
{
    string GenerateRefreshToken();

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}