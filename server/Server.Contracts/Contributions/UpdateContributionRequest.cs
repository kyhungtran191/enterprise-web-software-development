using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Contributions
{
    public class UpdateContributionRequest
    {
        public Guid ContributionId { get; set; }
        public string Title { get; set; }
        public Guid? AcademicYearId { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public List<IFormFile>? Files { get; set; }
        // will remove late - just for test
        public Guid FacultyId { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
