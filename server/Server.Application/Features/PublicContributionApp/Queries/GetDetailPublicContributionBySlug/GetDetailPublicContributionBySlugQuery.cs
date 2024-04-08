using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Wrappers;

namespace Server.Application.Features.PublicContributionApp.Queries.GetDetailPublicContributionBySlug
{
    public class GetDetailPublicContributionBySlugQuery : IRequest<ErrorOr<IResponseWrapper<PublicContributionWithCommentDto>>>
    {
        public string Slug { get; set; }
    }
}
