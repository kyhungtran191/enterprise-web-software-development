using Server.Domain.Entity.Content;

namespace Server.Application.Common.Dtos.Contributions
{
    public class ContributionInListDto
    { 
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public List<string> ThumbnailUrl { get; set; }
        public List<string> FilePath { get; set; }
        public ContributionStatus Status { get; set; }
        public string UserName { get; set; }
        public string FacultyName { get;set; }
        public string AcademicYear { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public DateTime? PublicDate { get; set; }

    }
}
