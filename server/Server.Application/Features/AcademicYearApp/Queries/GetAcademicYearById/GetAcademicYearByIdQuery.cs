using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.AcademicYears;
using Server.Application.Wrappers;

namespace Server.Application.Features.AcademicYearApp.Queries.GetAcademicYearById
{
    public class GetAcademicYearByIdQuery : IRequest<ErrorOr<IResponseWrapper<AcademicYearDto>>>
    {
        public Guid YearId { get; set; }
    }
}
