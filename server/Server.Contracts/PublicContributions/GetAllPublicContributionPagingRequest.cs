using Microsoft.AspNetCore.Mvc;
using Server.Contracts.Common;

namespace Server.Contracts.PublicContributions
{
    public class GetAllPublicContributionPagingRequest : PagingRequest
    {
        [FromQuery(Name = "year")] public string? Year { get; set; }
        [FromQuery(Name = "facultyname")] public string? FacultyName { get; set; }
        [FromQuery(Name = "status")] public string? Status { get; set; }
    }
}
