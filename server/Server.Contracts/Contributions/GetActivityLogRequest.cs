using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Contributions
{
    public class GetActivityLogRequest
    {
        [FromRoute]
        public Guid ContributionId { get; set; }
    }
}
