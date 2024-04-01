using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Domain.Entity.Content;

[Table("Contributions")]
[Index(nameof(Slug), IsUnique = true)]
public class Contribution : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid FacultyId { get; set; }
    [Required]
    public Guid AcademicYearId { get; set; }
    [Required]
    [Column(TypeName = "varchar(250)")]
    public required string Slug { get; set; }

    [Required]
    [MaxLength(256)] 
    public required string Title { get; set; }
    [Required]
    public required bool IsConfirmed { get; set; }
    public DateTime? SubmissionDate { get; set; }
    public DateTime? PublicDate { get; set; }
    public bool IsCoordinatorComment { get; set; } = false;
   
    public ContributionStatus Status { get; set; }
    public string Content { get; set; }
}
public enum ContributionStatus
{
    Pending,
    Approve,
    Reject

}

