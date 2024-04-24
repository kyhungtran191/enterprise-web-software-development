using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Server.Domain.Common.Converters;

namespace Server.Domain.Entity.Identity;

[Table("AppUsers")]
public class AppUser : IdentityUser<Guid>
{        
    public Guid? FacultyId { get; set; }

    //[Required]
    [MaxLength(100)]
    public string? FirstName { get; set; } = default!;

    //[Required]
    [MaxLength(100)]
    public string? LastName { get; set; } = default!;

    public bool IsActive { get; set; }
    public DateTime? Dob { get; set; }
    public DateTime? DateCreated { get; set; }
    public string? RefreshToken { get; set; }
    public string? AccessToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public bool IsOnline { get; set; }

    [MaxLength(500)]
    public string? Avatar { get; set; }
    [MaxLength(500)]
    public string? AvatarPublicId { get; set; }
}