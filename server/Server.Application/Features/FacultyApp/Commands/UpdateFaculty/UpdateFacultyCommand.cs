using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.FacultyApp.Commands.UpdateFaculty;

public class UpdateFacultyCommand : IRequest<ErrorOr<IResponseWrapper>>
{
    public Guid FacultyId { get; set; }
    public string NewFacultyName { get; set; } = default!;
    public string Icon { get; set; } = default!;
}