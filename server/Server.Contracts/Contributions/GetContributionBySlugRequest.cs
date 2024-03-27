using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Contributions
{
    public class GetContributionBySlugRequest
    {
        [FromRoute]
        public string Slug { get; set; }
    }
}
