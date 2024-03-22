using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Faculties;
using Server.Application.Wrappers;

namespace Server.Application.Features.FacultyApp.Queries.GetFacultyByName;

public class GetFacultyByIdQuery : IRequest<ErrorOr<IResponseWrapper<FacultyDto>>>
{
    public Guid FacultyId { get; set; }
}