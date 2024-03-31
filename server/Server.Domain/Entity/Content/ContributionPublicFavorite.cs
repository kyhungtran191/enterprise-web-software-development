using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Entity.Content
{
    [Table("ContributionPublicFavorites")]
    public class ContributionPublicFavorite : BaseEntity
    {
        public Guid ContributionPublicId { get; set; }
        public Guid UserId {get; set;}
    }
}
