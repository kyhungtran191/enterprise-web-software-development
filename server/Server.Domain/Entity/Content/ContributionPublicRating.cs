using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Entity.Content
{
    [Table("ContributionPublicRatings")]
    public class ContributionPublicRating : BaseEntity
    {
        public Guid ContributionPublicId { get; set; }
        public Guid UserId { get; set; }
        public double Rating { get; set; } = 0;
    }
}
