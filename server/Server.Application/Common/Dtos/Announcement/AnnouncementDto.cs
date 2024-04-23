using System.ComponentModel.DataAnnotations;

namespace Server.Application.Common.Dtos.Announcement
{
  public class AnnouncementDto
  {
    public string? Id { get; set; }

    [Required]
    [StringLength(250)]
    public string? Title { set; get; }

    [StringLength(250)]
    public string? Content { set; get; }

    public Guid UserId { set; get; }
    public string? Username { get; set; }

    public DateTime DateCreated { set; get; }
    public DateTime DateModified { set; get; }
    public string? Type { get; set; }
    public string? Avatar { get; set; }
    public string? Slug { get; set; }
  }
}
