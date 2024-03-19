using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.Identity.Users.Commands.DeleteUserById;

public class DeleteUserByIdCommand : IRequest<ErrorOr<IResponseWrapper>>
{
    public Guid Id { get; set; }
}