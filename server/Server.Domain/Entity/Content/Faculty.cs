
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Entity.Content
{
    [Table("Faculties")]
    public class Faculty : BaseEntity
    {
        [Required]
        [MaxLength(256)]
        public required string Name { get; set; }

        [MaxLength(2048)]
        public string Icon { get; set; } = default!;
    }
}
