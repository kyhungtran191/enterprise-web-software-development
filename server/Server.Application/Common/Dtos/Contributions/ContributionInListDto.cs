using Server.Domain.Entity.Content;

namespace Server.Application.Common.Dtos.Contributions
{
    public class ContributionInListDto
    { 
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public List<FileReturnDto> Thumbnails { get; set; }
        public List<FileReturnDto> Files { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public string FacultyName { get;set; }
        public string AcademicYear { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public DateTime? PublicDate { get; set; }

    }
}
