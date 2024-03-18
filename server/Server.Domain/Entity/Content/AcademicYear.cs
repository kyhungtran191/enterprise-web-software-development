
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Domain.Entity.Content;

[Table("AcademicYears")]
public class AcademicYear : BaseEntity
{
    public DateTime StartClosureDate { get; set; }
    public DateTime EndClosureDate { get; set; }
    public DateTime FinalClosureDate { get; set; }

    [MaxLength(256)]
    public required string UserNameCreated { get; set; }
}
