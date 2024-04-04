using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Contributions
{
    public class UpdateContributionRequest
    {
        public Guid ContributionId { get; set; }
        public string? Title { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public List<IFormFile>? Files { get; set; }
        public bool IsConfirmed { get; set; } = false;
        public string Content { get; set; }
        public string ShortDescription { get; set; }
    }
}
