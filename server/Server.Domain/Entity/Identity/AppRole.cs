using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Server.Domain.Entity.Identity;

[Table("AppRoles")]
public class AppRole : IdentityRole<Guid>
{
    public string DisplayName { get; set; } = default!;

}