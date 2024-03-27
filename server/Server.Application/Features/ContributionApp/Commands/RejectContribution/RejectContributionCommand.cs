using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.ContributionApp.Commands.RejectContribution
{
    public class RejectContributionCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Note { get; set; }
    }
}
