
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Domain.Entity.Content;

[Table("AcademicYears")]
[Index(nameof(Name),IsUnique = true)]
public class AcademicYear : BaseEntity
{
    [Required]
    public required string Name { get; set; }
    public DateTime StartClosureDate { get; set; }
    public DateTime EndClosureDate { get; set; }
    public DateTime FinalClosureDate { get; set; }

    [MaxLength(256)]
    public required string UserNameCreated { get; set; }
}
