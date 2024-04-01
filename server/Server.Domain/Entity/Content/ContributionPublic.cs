using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Server.Domain.Entity.Content
{
    [Table("ContributionPublics")]
    public class ContributionPublic : BaseEntity
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
        [MaxLength(256)]
        public string UserName { get; set; } = default!;

        [MaxLength(500)]
        public string Avatar { get; set; } = default!;
        public string FacultyName { get; set; } = default!;
                
        public int LikeQuantity { get; set; } = 0;
                
        public int Views { get; set; } = 0;
        public string Content { get; set; }
        public string ShortDescription { get; set; }
    }
}
