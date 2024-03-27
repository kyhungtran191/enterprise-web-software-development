using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Contributions
{
    public class UpdateContributionRequest
    {
        public Guid ContributionId { get; set; }
        public string? Title { get; set; }
        public Guid? AcademicYearId { get; set; }
        public string? Thumbnail { get; set; }
        public string? FilePath { get; set; }
    }
}
