using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.AcademicYearApp.Queries.ActiveYear
{
    public class ActiveYearCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public Guid YearId { get; set; }
    }
}
