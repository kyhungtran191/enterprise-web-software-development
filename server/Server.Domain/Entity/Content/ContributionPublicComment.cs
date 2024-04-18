using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Entity.Content
{
    [Table("ContributionPublicComments")]
    public class ContributionPublicComment : BaseEntity
    {
        public Guid ContributionId { get; set; }
        public Guid UserId { get; set; }

        [Required]
        public string Content { get; set; } = default!;
    }
}
