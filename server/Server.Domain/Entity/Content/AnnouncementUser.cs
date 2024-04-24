using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Entity.Content;

[Table("AnnouncementUsers")]
public class AnnouncementUser
{
    public AnnouncementUser() { }

    public AnnouncementUser(string announcementId, Guid userId, bool? hasRead)
    {
        AnnouncementId = announcementId;
        UserId = userId;
        HasRead = hasRead;
    }

    [Key]
    public int Id { get; set; }

    [StringLength(128)]
    [Required]
    public string AnnouncementId { get; set; }

    public Guid? UserId { get; set; }

    public bool? HasRead { get; set; }
}