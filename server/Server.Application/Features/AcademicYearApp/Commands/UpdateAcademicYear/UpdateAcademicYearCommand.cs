using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.AcademicYearApp.Commands.UpdateAcademicYear
{
    public class UpdateAcademicYearCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public Guid AcademicYearId { get; set; }
        public string AcademicYearName { get; set; } = null!;
        public DateTime StartClosureDate { get; set; }
        public DateTime EndClosureDate { get; set; }
        public DateTime FinalClosureDate { get; set; }
    }
}
