using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.ContributionApp.Commands.ApproveContributions
{
    public class ApproveContributionsCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public List<Guid> Ids { get; set; }
        public Guid UserId { get; set; }
    }
}
