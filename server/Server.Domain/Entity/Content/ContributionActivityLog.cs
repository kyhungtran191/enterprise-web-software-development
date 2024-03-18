using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Entity.Content
{
    [Table("ContributionActivityLogs")]
    public class ContributionActivityLog : BaseEntity
    {
        public Guid UserId { get; set; }
        
        [MaxLength(255)]
        public string Title { get; set; } = default!;

        [MaxLength(500)]
        public string Description { get; set; } = default!;
        public string UserName { get; set; } = default!;
        
        [Required]
        public ContributionStatus FromStatus { get; set;}
        
        [Required]
        public ContributionStatus ToStatus { get;set;}
    }
}
