using Microsoft.AspNetCore.Mvc;
using Server.Contracts.Common;

namespace Server.Contracts.PublicContributions
{
    public class GetAllPublicContributionPagingRequest : PagingRequest
    {
        [FromQuery] public string? Year { get; set; }
        [FromQuery] public string? FacultyName { get; set; }
        [FromQuery] public string? Status { get; set; }
    }
}
