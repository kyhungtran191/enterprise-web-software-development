using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.FacultyApp.Commands.DeleteFaculty;

public class DeleteFacultyCommand : IRequest<ErrorOr<IResponseWrapper>>
{
    public Guid FacultyId { get; set; }   
}