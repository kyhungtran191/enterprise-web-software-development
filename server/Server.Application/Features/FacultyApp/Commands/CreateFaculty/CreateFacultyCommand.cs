using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.FacultyApp.Commands.CreateFaculty;

public class CreateFacultyCommand : IRequest<ErrorOr<IResponseWrapper>>
{
    public string FacultyName { get; set; } = default!;
    public string Icon { get; set; } = default!;
}