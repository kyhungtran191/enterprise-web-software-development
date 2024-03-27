using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Contributions
{
    public class GetRejectReasonRequest
    {
        [FromRoute]
        public Guid Id { get; set; }
    }
}
