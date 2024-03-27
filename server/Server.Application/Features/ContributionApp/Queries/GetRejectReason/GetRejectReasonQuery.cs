using ErrorOr;
using MediatR;
using Server.Application.Wrappers;

namespace Server.Application.Features.ContributionApp.Queries.GetRejectReason
{
    public class GetRejectReasonQuery : IRequest<ErrorOr<IResponseWrapper<string>>>
    {
        public Guid Id { get; set; }
    }
}
