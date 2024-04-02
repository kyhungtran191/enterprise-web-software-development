using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.AcademicYearApp.Queries.InactiveYear
{
    public class InactiveYearCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public Guid YearId { get; set; }
    }
}
