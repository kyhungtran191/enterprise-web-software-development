using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Entity.Content;

[Table("Announcements")]    
public class Announcement
{
    [Key]
    public string Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Title { set; get; }

    [StringLength(250)]
    public string? Content { set; get; }

    [StringLength(500)]
    public string? Slug { set; get; }

    
    [StringLength(500)]
    public string? Type { set; get; }
    
    [StringLength(255)]
    public string? Username { set; get; }

    [StringLength(500)]
    public string? Avatar { get; set; }    

    public Guid UserId { set; get; }

    public DateTime DateCreated { set; get; }
    public DateTime DateModified { set; get; }
}