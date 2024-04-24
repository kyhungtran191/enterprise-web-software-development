using ErrorOr;
using MediatR;
using Server.Application.Common.Dtos.Contributions;
using Server.Application.Wrappers;
using Server.Application.Wrappers.PagedResult;
using Server.Contracts.Common;

namespace Server.Application.Features.ContributionApp.Queries.GetNotCommentContribution
{
    public class GetNotCommentContributionQuery : PagingRequest, IRequest<ErrorOr<IResponseWrapper<PagedResult<NotCommentContributionDto>>>>
    {

    }
}
