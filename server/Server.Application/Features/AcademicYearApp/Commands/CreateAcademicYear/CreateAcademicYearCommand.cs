using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.AcademicYearApp.Commands.CreateAcademicYear
{
    public class CreateAcademicYearCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public string AcademicYearName { get; set; } = null!;
        public DateTime StartClosureDate { get; set; }
        public DateTime EndClosureDate { get; set; }
        public DateTime FinalClosureDate { get; set; }
        public string UserNameCreated { get; set; } = null!;
    }
}
