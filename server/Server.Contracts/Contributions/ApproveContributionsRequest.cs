using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Contributions
{
    public class ApproveContributionsRequest
    {
        public List<Guid> Ids { get; set; }
    }
}
