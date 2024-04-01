using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Wrappers;

namespace Server.Application.Features.ContributionApp.Queries.GetTopContributors
{
    public class GetTopContributorsQuery : IRequest<ErrorOr<IResponseWrapper<List<TopContributorDto>>>>
    {
    }
}
