using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Entity.Content
{
    [Table("Likes")]
    public class Like : BaseEntity
    {
        public Guid ContributionPublicId { get; set; }
        public Guid UserId { get; set; }
    }
}
