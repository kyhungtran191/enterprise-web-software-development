
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Domain.Entity.Content
{
    [Table("Faculties")]
    [PrimaryKey(nameof(Name))]
    public class Faculty
    {
        [Required]
        [MaxLength(256)]
        public required string Name { get; set; }

        [MaxLength(2048)]
        public string Icon { get; set; } = default!;

        public DateTime DateCreated { get; set; }
        public DateTime? DateEdited { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
