using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Queries.GetTop4Contributions
{
    public class GetTop4ContributionQuery : IRequest<ErrorOr<IResponseWrapper<List<PublicContributionInListDto>>>>
    {
    }
}
