using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.AcademicYearApp.Commands.DeleteAcademicYear
{
    public class DeleteAcademicYearCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public List<Guid> YearIds { get; set; }
    }
}
