using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Server.Application.Common.Dtos;
using Server.Domain.Common.Constants;
using Server.Domain.Entity.Identity;

namespace Server.Application.Common.Extensions;

public static class ClaimExtensions
{
    public static void GetPermissionsByType(this List<RoleClaimsDto> allPermissions, Type policy)
    {
        FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);

        foreach (FieldInfo field in fields)
        {
            string displayName = field.GetValue(null)!.ToString()!;
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), true);

            if (attributes.Length > 0)
            {
                var description = (DescriptionAttribute)attributes[0];
                displayName = description.Description;
            }

            allPermissions.Add(new RoleClaimsDto
            {
                Value = field.GetValue(null)!.ToString()!,
                Type = UserClaims.Permissions,
                DisplayName = displayName,
            });
        }
    }

    public static async Task AddPermissionClaim(this RoleManager<AppRole> roleManager, AppRole appRole, string permission)
    {
        var allClaims = await roleManager.GetClaimsAsync(appRole);

        if (!allClaims.Any(p => p.ValueType == UserClaims.Permissions && p.Value == permission))
        {
            await roleManager.AddClaimAsync(appRole, new Claim(UserClaims.Permissions, permission));
        }
    }
}