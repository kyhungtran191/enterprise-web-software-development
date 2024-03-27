using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Domain.Entity.Content
{
    [Table("Tags")]
    [Index(nameof(Name),IsUnique = true)]
    public class Tag : BaseEntity
    {
        [Required]
        [MaxLength(256)]
        public required string Name { get; set; }
    }
}
