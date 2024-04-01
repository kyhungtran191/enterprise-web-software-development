using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Users;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Queries.GetListUserLiked
{
    public class GetListUserLikedQuery : IRequest<ErrorOr<IResponseWrapper<List<UserInListDto>>>>
    {
        public Guid ContributionId { get; set; }
    }
}
