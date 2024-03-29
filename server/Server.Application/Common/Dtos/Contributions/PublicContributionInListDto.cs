namespace Server.Application.Common.Dtos.Contributions
{
    public class PublicContributionInListDto
    {
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public List<FileReturnDto> Thumbnails { get; set; }
        public string UserName { get; set; }
        public string FacultyName { get; set; }
        public string AcademicYear { get; set; }
        public DateTime? PublicDate { get; set; }
        public DateTime? DateEdited { get; set; }
    }
}
