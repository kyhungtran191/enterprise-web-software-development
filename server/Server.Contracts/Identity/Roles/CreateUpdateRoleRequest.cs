namespace Server.Contracts.Identity.Roles;

public class CreateUpdateRoleRequest
{
  public required string Name { get; set; }
  public required string DisplayName { get; set; }
}

