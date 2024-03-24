using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.AcademicYears
{
    public class GetYearByIdRequest
    {
        [FromRoute]
        public Guid YearId { get; set; }
    }
}
