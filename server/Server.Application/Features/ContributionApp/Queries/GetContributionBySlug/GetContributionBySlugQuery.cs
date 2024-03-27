using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Wrappers;

namespace Server.Application.Features.ContributionApp.Queries.GetContributionByTitle
{
    public class GetContributionBySlugQuery : IRequest<ErrorOr<IResponseWrapper<ContributionDto>>>
    {
        public string Slug { get; set; }
    }
}
