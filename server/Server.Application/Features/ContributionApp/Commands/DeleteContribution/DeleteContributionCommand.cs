using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.ContributionApp.Commands.DeleteContribution
{
    public class DeleteContributionCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public List<Guid> ContributionIds { get; set; }
    }
}
