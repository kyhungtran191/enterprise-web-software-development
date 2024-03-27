using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Tags
{
    public class GetTagByIdRequest
    {
        [FromRoute]
        public Guid TagId { get; set; }
    }
}
