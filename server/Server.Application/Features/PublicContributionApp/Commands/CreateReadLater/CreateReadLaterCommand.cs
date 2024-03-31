using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Commands.CreateReadLater
{
    public class CreateReadLaterCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public Guid ContributionId { get; set; }
        public Guid UserId { get; set; }
    }
}
