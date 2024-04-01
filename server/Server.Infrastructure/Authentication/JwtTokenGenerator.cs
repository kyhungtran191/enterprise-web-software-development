using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Application.Common.Dtos;
using Server.Application.Common.Extensions;
using Server.Application.Common.Interfaces.Authentication;
using Server.Application.Common.Interfaces.Persistence;
using Server.Application.Common.Interfaces.Services;
using Server.Domain.Common.Constants;
using Server.Domain.Entity.Identity;

namespace Server.Infrastructure.Authentication;

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IFacultyRepository _facultyRepository;

    public JwtTokenGenerator(IDateTimeProvider dateTimeProvider,
                             IOptions<JwtSettings> jwtSettings,
                             UserManager<AppUser> userManager,
                             RoleManager<AppRole> roleManager,
                             IFacultyRepository facultyRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
        _roleManager = roleManager;
        _facultyRepository = facultyRepository;
    }

    public async Task<string> GenerateToken(AppUser applicationUser)
    {
        var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaims(applicationUser));

        return token;
    }

    private async Task<Claim[]> GetClaims(AppUser applicationUser)
    {
        var roles = await _userManager.GetRolesAsync(applicationUser);
        var permissions = await GetPermissions(roles.ToHashSet());

        string facultyName = string.Empty;
        if (applicationUser.FacultyId is not null) {

            var facultyFromDb = await _facultyRepository.GetByIdAsync(applicationUser.FacultyId.Value);
            facultyName = facultyFromDb.Name;
        }

        return new Claim[] {
             // Subject (Principal)
            new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id.ToString()),
            new Claim(UserClaims.Id, applicationUser.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, applicationUser.UserName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, applicationUser.UserName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.GivenName, applicationUser.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, applicationUser.LastName),
            new Claim(UserClaims.Roles, string.Join(";", roles)),
            new Claim(UserClaims.Permissions, JsonSerializer.Serialize(permissions)),
            new Claim(UserClaims.FacultyId, applicationUser.FacultyId.ToString()),
            new Claim(UserClaims.FacultyName, facultyName),
            // Jwt ID
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
    }

    private async Task<List<string>> GetPermissions(HashSet<string> roles)
    {
        var permissions = new List<string>();

        if (roles.Contains(Roles.Admin))
        {
            var allPermisisons = new List<RoleClaimsDto>();

            var types = 
                typeof(Permissions)
                .GetTypeInfo()
                .DeclaredNestedTypes
                .ToList();

            types.ForEach(allPermisisons.GetPermissionsByType);

            if (allPermisisons.Any())
            {
                permissions = allPermisisons.ConvertAll(RoleClaimDto => RoleClaimDto.Value!);
            }
        }
        else
        {
            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role is not null)
                {
                    var claims = await _roleManager.GetClaimsAsync(role);
                    permissions.AddRange(claims.Select(claim => claim.Value));
                }
            }
        }

        return permissions.Distinct().ToList();
    }

    private SigningCredentials GetSigningCredentials()
    {
        var secret = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
            claims: claims,
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: signingCredentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        var encryptedToken = tokenHandler.WriteToken(token);

        return encryptedToken;
    }
}
