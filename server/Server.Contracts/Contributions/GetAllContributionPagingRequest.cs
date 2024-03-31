using Microsoft.AspNetCore.Mvc;
using Server.Contracts.Common;

namespace Server.Contracts.Contributions
{
    public class GetAllContributionPagingRequest : PagingRequest
    {
        [FromQuery] public string? Year { get; set; }
        [FromQuery] public string? FacultyName { get; set; }
        [FromQuery] public string? Status { get; set; }
    }
}
