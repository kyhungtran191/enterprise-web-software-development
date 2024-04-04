using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Commands.AllowGuest
{
    public class AllowGuestCommand : IRequest<ErrorOr<IResponseWrapper>>
    {
        public List<Guid> Ids { get; set; }
    }
}
