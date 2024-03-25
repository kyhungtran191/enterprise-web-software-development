using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Wrappers;

namespace Server.Application.Features.ContributionApp.Queries.GetContributionByTitle
{
    public class GetContributionByTitleQuery : IRequest<ErrorOr<IResponseWrapper<ContributionDto>>>
    {
        public string Title { get; set; }
    }
}
