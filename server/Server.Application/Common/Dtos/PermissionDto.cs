namespace Server.Application.Common.Dtos;
public class PermissionDto
{
    public string RoleId { get; set; } = string.Empty;
    public IList<RoleClaimsDto> RoleClaims { get; set; } = new List<RoleClaimsDto>();
}