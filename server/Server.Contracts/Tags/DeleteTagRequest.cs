using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.Tags
{
    public class DeleteTagRequest
    {
        [FromRoute]
        public List<Guid> TagIds { get; set; } = new List<Guid>();
    }
}
