using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.PublicContributions.Like
{
    public class LikeContributionRequest
    {
       [FromRoute] public Guid ContributionId { get; set; }
    }
}
