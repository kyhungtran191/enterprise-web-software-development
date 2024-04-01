using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Queries.GetTopContribution
{
    public class GetFeaturedContributionQuery : IRequest<ErrorOr<IResponseWrapper<List<PublicContributionInListDto>>>>
    {
    }
}
