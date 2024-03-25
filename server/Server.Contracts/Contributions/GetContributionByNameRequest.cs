using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Contributions
{
    public class GetContributionByTitleRequest
    {
        [FromRoute]
        public string Title { get; set; }
    }
}
