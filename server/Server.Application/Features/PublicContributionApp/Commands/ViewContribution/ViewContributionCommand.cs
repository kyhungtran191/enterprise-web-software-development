using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Commands.ViewContribution
{
    public class ViewContributionCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public Guid ContributionId { get; set; }
    }
}
