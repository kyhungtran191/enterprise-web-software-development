using Microsoft.AspNetCore.Mvc;
using Server.Contracts.Common;

namespace Server.Contracts.Contributions
{
    public class GetFacultyContributionRequest : PagingRequest
    {
        [FromQuery(Name = "year")] public string? Year { get; set; }
        [FromQuery(Name = "status")] public string? Status { get; set; }
       
    }
}
