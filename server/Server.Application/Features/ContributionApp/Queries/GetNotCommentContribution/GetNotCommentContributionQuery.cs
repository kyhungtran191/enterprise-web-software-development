using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Wrappers;
using Server.Application.Wrappers.PagedResult;

namespace Server.Application.Features.ContributionApp.Queries.GetNotCommentContribution
{
    public class GetNotCommentContributionQuery : IRequest<ErrorOr<IResponseWrapper<PagedResult<NotCommentContributionDto>>>>
    {

    }
}
