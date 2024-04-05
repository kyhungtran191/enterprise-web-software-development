using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Entity.Content
{
    [Table("ContributionPublicComments")]
    [PrimaryKey(nameof(ContributionId), nameof(UserId))]
    public class ContributionPublicComment
    {
        public Guid ContributionId { get; set; }
        public Guid UserId { get; set; }

        [Required]
        public string Content { get; set; } = default!;
        public DateTime DateCreated { get; set; }
        public DateTime DateEdited { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
