using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Users;
using Server.Application.Wrappers;

namespace Server.Application.Features.Identity.Users.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<ErrorOr<IResponseWrapper<UserDto>>>
{
    public Guid Id { get; set; }
}