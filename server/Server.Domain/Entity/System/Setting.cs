using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Entity.System
{
    [Table("Settings")]
    public class Setting : BaseEntity
    {
        public Guid? UserId { get; set; }
        public bool? IsGive { get; set; }
        public DateTime? FinalClosureDate { get; set; }
    }
}
