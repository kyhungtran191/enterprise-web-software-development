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

        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }        
    }
}
