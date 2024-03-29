using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Users;
using Server.Application.Wrappers;

namespace Server.Application.Features.Identity.Users.Queries.GetProfile
{
    public class GetProfileQuery : IRequest<ErrorOr<IResponseWrapper<UserProfileDto>>>
    {
        public Guid UserId { get; set; }
    }
}
