using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Commands.LikeContribution
{
    public class LikeContributionCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public Guid ContributionId { get; set; }
        public Guid UserId { get; set; }
    }
}
