using MediatR;
using Server.Application.Common.Dtos.Contributions;
using ErrorOr;
using Server.Application.Wrappers;

namespace Server.Application.Features.ContributionApp.Queries.GetUserContribution
{
    public class GetUserContributionQuery : IRequest<ErrorOr<IResponseWrapper<ContributionDto>>>
    {
        public string Slug { get; set; }
        public Guid UserId { get; set; }
    }
}
