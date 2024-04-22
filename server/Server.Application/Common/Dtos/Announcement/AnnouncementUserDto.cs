using System.ComponentModel.DataAnnotations;

namespace Server.Application.Common.Dtos.Announcement
{
       public class AnnouncementUserDto
    {
        public int Id { set; get; }

        [StringLength(128)]
        [Required]
        public string? AnnouncementId { get; set; }

        public Guid UserId { get; set; }

        public bool? HasRead { get; set; }
    }
   
}
