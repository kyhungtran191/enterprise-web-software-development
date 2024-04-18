using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Domain.Entity.Content
{
    [Table("ContributionComments")]
    public class ContributionComment : BaseEntity
    {
        public Guid ContributionId { get; set; }
        public Guid UserId { get; set; }

        [Required]
        public string Content { get; set; } = default!;
    }
}
