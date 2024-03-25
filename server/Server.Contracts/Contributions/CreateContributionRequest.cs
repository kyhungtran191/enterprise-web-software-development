using Microsoft.AspNetCore.Http;

namespace Server.Contracts.Contributions
{
    public class CreateContributionRequest
    {
        public required string Title { get; set; }
        public string? ThumbnailUrl { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public string? FilePath { get; set; }
        public IFormFile? File { get; set; }
        public Guid AcademicYearId { get; set; }

        // will remove late - just for test
        public Guid FacultyId { get; set; }
        
        // tags
        public List<Guid>? TagId { get; set; }

    }
}
