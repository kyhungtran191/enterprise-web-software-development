using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Domain.Entity.Content
{
    [Table("ContributionComments")]
    [PrimaryKey(nameof(ContributionId), nameof(UserId))]
    public class ContributionComment
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
