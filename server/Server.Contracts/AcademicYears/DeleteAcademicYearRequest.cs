using Microsoft.AspNetCore.Mvc;

namespace Server.Contracts.AcademicYears
{
    public class DeleteAcademicYearRequest
    {
        [FromRoute]
        public List<Guid> YearIds { get; set; }
    }
}
