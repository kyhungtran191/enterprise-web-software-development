using Microsoft.AspNetCore.Mvc;
using Server.Contracts.Common;

namespace Server.Contracts.Contributions
{
    public class GetAllContributionPagingRequest : PagingRequest
    {
        [FromQuery] public Guid? AcademicYearId { get; set; }
        [FromQuery] public Guid? FacultyId { get; set; }
    }
}
