using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Application.Common.Interfaces.Authentication;

namespace Server.Infrastructure.Authentication;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    /// <summary>
    /// Get user principle from expired token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="SecurityTokenException"></exception>
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true, //you might want to validate the audience and issuer depending on your use case
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = _jwtSettings.Audience,
            ValidIssuer = _jwtSettings.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            ValidateLifetime = false //here we are saying that we don't care about the token's expiration date,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}